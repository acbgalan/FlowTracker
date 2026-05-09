import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
  inject,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryResponse } from '../../../core/models/category/categoryResponse.interface';
import { QueryParametersInterface } from '../../../core/models/common/queryParameters.interface';
import { SavingGoalResponse } from '../../../core/models/savingGoal/saving-goal-response.interface';
import { Type } from '../../../core/models/enums/type.enum';
import { TransactionResponse } from '../../../core/models/transaction/transactionResponse.interface';
import { UpdateTransactionRequest } from '../../../core/models/transaction/updateTransactionRequest.interface';
import { SavingGoalService } from '../../../core/services/savingGoal.service';
import { TransactionService } from '../../../core/services/transaction.service';
import { CategoryService } from '../../../core/services/category.service';

@Component({
  selector: 'app-edit-transaction-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-transaction-modal.html',
  styleUrl: './edit-transaction-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditTransactionModal implements OnInit, OnChanges, OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private transactionService = inject(TransactionService);
  private categoryService = inject(CategoryService);
  private savingGoalService = inject(SavingGoalService);

  @Input() public transaction: TransactionResponse | null = null;
  @Output() public updated = new EventEmitter<void>();
  public isSubmitting = false;
  public isLoadingCategories = false;
  public isLoadingSavingGoals = false;
  public errorMessage = '';
  public categoriesError = '';
  public savingGoalsError = '';
  public showSuccessToast = false;
  public categories: CategoryResponse[] = [];
  public savingGoals: SavingGoalResponse[] = [];

  public readonly form = this.formBuilder.nonNullable.group({
    date: ['', Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    savingGoalId: [0],
    description: ['', [Validators.maxLength(300)]],
  });

  public ngOnInit(): void {
    this.loadCategories();
    this.loadSavingGoals();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['transaction']) {
      this.syncFormWithTransaction();
    }
  }

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onSubmit(): void {
    const selectedTransaction = this.transaction;

    if (!selectedTransaction || this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const descriptionValue = rawValue.description.trim();
    const request: UpdateTransactionRequest = {
      id: selectedTransaction.id,
      amount: Number(rawValue.amount),
      date: rawValue.date,
      description: descriptionValue.length > 0 ? descriptionValue : null,
      categoryId: Number(rawValue.categoryId),
      savingGoalId: this.isSavingCategorySelected() ? Number(rawValue.savingGoalId) : null,
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.transactionService.updateTransaction(selectedTransaction.id, request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.updated.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo actualizar la transacción. Intenta nuevamente.';
      },
    });
  }

  private loadCategories(): void {
    const queryParams: QueryParametersInterface = {
      searchTerm: null,
      sortBy: 'name',
      sortDesc: false,
      page: 1,
      limit: 500,
    };

    this.isLoadingCategories = true;
    this.categoriesError = '';

    this.categoryService.getCategories(queryParams).subscribe({
      next: response => {
        this.categories = response.data;
        this.isLoadingCategories = false;
        this.syncFormWithTransaction();
      },
      error: () => {
        this.categories = [];
        this.isLoadingCategories = false;
        this.categoriesError = 'No se pudieron cargar las categorías.';
      },
    });
  }

  private loadSavingGoals(): void {
    this.isLoadingSavingGoals = true;
    this.savingGoalsError = '';

    this.savingGoalService.getSavingGoals().subscribe({
      next: response => {
        this.savingGoals = response;
        this.isLoadingSavingGoals = false;
        this.syncSavingGoalState();
      },
      error: () => {
        this.savingGoals = [];
        this.isLoadingSavingGoals = false;
        this.savingGoalsError = 'No se pudieron cargar las metas de ahorro.';
        this.syncSavingGoalState();
      },
    });
  }

  private syncFormWithTransaction(): void {
    const selectedTransaction = this.transaction;

    if (!selectedTransaction) {
      this.form.reset({
        date: '',
        amount: 0,
        categoryId: 0,
        savingGoalId: 0,
        description: '',
      });
      this.errorMessage = '';
      this.isSubmitting = false;
      this.syncSavingGoalState();
      return;
    }

    const categoryId =
      this.findCategoryIdByName(selectedTransaction.categoryName) ||
      0;

    this.form.reset({
      date: this.toDateInputValue(selectedTransaction.date),
      amount: selectedTransaction.amount,
      categoryId,
      savingGoalId: selectedTransaction.savingGoalId ?? 0,
      description: selectedTransaction.description ?? '',
    });
    this.errorMessage = '';
    this.isSubmitting = false;
    this.syncSavingGoalState();
  }

  private findCategoryIdByName(name: string): number | null {
    const match = this.categories.find(category => category.name === name);
    return match ? match.id : null;
  }

  private findCategoryById(categoryId: number): CategoryResponse | undefined {
    return this.categories.find(category => category.id === categoryId);
  }

  public isSavingCategorySelected(): boolean {
    const categoryId = Number(this.form.controls.categoryId.value);
    const selectedCategory = this.findCategoryById(categoryId);
    return selectedCategory?.type === Type.Saving;
  }

  private syncSavingGoalState(): void {
    if (!this.isSavingCategorySelected()) {
      this.form.controls.savingGoalId.setValue(0);
      this.form.controls.savingGoalId.clearValidators();
      this.form.controls.savingGoalId.updateValueAndValidity({ emitEvent: false });
      return;
    }

    this.form.controls.savingGoalId.setValidators([Validators.required, Validators.min(1)]);
    this.form.controls.savingGoalId.updateValueAndValidity({ emitEvent: false });
  }

  private toDateInputValue(value: string | Date): string {
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
