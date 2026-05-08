import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges,
  ViewChild,
  inject,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { SavingGoalResponse } from '../../../core/models/savingGoal/saving-goal-response.interface';
import { UpdateSavingGoalRequest } from '../../../core/models/savingGoal/update-saving-goal-request.interface';
import { SavingGoalService } from '../../../core/services/savingGoal.service';

@Component({
  selector: 'app-edit-saving-goal-modal',
  standalone: false,
  templateUrl: './edit-saving-goal-modal.html',
  styleUrl: './edit-saving-goal-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditSavingGoalModal implements OnChanges, OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private savingGoalService = inject(SavingGoalService);

  @Input() public savingGoal: SavingGoalResponse | null = null;
  @Output() public updated = new EventEmitter<void>();
  public isSubmitting = false;
  public errorMessage = '';
  public showSuccessToast = false;

  public readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    targetAmount: [0, [Validators.required, Validators.min(0.01)]],
    deadline: [''],
  });

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['savingGoal']) {
      this.syncFormWithGoal();
    }
  }

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onSubmit(): void {
    const selectedGoal = this.savingGoal;

    if (!selectedGoal || this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const request: UpdateSavingGoalRequest = {
      id: selectedGoal.id,
      name: rawValue.name.trim(),
      targetAmount: Number(rawValue.targetAmount),
      deadline: rawValue.deadline ? rawValue.deadline : null,
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.savingGoalService.updateSavingGoal(selectedGoal.id, request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.updated.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo actualizar la meta de ahorro. Intenta nuevamente.';
      },
    });
  }

  private syncFormWithGoal(): void {
    const selectedGoal = this.savingGoal;

    if (!selectedGoal) {
      this.form.reset({
        name: '',
        targetAmount: 0,
        deadline: '',
      });
      this.errorMessage = '';
      this.isSubmitting = false;
      return;
    }

    this.form.reset({
      name: selectedGoal.name,
      targetAmount: selectedGoal.targetAmount,
      deadline: this.toDateInputValue(selectedGoal.deadline),
    });
    this.errorMessage = '';
    this.isSubmitting = false;
  }

  private toDateInputValue(value: string | Date | null | undefined): string {
    if (!value) {
      return '';
    }
    const dateValue = new Date(value);
    if (Number.isNaN(dateValue.getTime())) {
      return '';
    }
    return dateValue.toISOString().split('T')[0];
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
