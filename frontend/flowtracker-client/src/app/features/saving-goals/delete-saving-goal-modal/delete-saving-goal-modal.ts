import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  ViewChild,
  inject,
} from '@angular/core';
import { SavingGoalResponse } from '../../../core/models/savingGoal/saving-goal-response.interface';
import { SavingGoalService } from '../../../core/services/savingGoal.service';

@Component({
  selector: 'app-delete-saving-goal-modal',
  standalone: false,
  templateUrl: './delete-saving-goal-modal.html',
  styleUrl: './delete-saving-goal-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteSavingGoalModal implements OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private savingGoalService = inject(SavingGoalService);

  @Input() public savingGoal: SavingGoalResponse | null = null;
  @Output() public deleted = new EventEmitter<void>();
  public isDeleting = false;
  public errorMessage = '';
  public showSuccessToast = false;

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onDelete(): void {
    const selectedGoal = this.savingGoal;

    if (!selectedGoal || this.isDeleting) {
      return;
    }

    this.isDeleting = true;
    this.errorMessage = '';

    this.savingGoalService.deleteSavingGoal(selectedGoal.id).subscribe({
      next: () => {
        this.isDeleting = false;
        this.deleted.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'No se pudo eliminar la meta de ahorro. Intenta nuevamente.';
      },
    });
  }

  private hideModal(): void {
    this.dismissButton.nativeElement.click();
  }

  private showSuccessToastMessage(): void {
    this.showSuccessToast = true;

    if (this.toastTimeoutId !== null) {
      window.clearTimeout(this.toastTimeoutId);
    }

    this.toastTimeoutId = window.setTimeout(() => {
      this.showSuccessToast = false;
      this.toastTimeoutId = null;
    }, 3000);
  }

  public ngOnDestroy(): void {
    if (this.toastTimeoutId !== null) {
      window.clearTimeout(this.toastTimeoutId);
      this.toastTimeoutId = null;
    }
  }
}
