import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  Output,
  ViewChild,
  inject,
} from '@angular/core';
import { ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { SavingGoalService } from '../../../core/services/savingGoal.service';
import { CreateSavingGoalRequest } from '../../../core/models/savingGoal/create-saving-goal-request.interface';

@Component({
  selector: 'app-create-saving-goal-modal',
  standalone: false,
  templateUrl: './create-saving-goal-modal.html',
  styleUrl: './create-saving-goal-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateSavingGoalModal implements OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private savingGoalService = inject(SavingGoalService);

  @Output() public created = new EventEmitter<void>();
  public isSubmitting = false;
  public errorMessage = '';
  public showSuccessToast = false;

  public readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    targetAmount: [0, [Validators.required, Validators.min(0.01)]],
    deadline: [''],
  });

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const request: CreateSavingGoalRequest = {
      name: rawValue.name.trim(),
      targetAmount: Number(rawValue.targetAmount),
      deadline: rawValue.deadline ? rawValue.deadline : null,
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.savingGoalService.createSavingGoal(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.form.reset({
          name: '',
          targetAmount: 0,
          deadline: '',
        });
        this.created.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo crear la meta de ahorro. Intenta nuevamente.';
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
