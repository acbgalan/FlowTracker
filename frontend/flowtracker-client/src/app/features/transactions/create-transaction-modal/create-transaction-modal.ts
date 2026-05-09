import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
  inject,
} from '@angular/core';
import { ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { TransactionService } from '../../../core/services/transaction.service';
import { CategoryService } from '../../../core/services/category.service';
import { CategoryResponse } from '../../../core/models/category/categoryResponse.interface';
import { CreateTransactionRequest } from '../../../core/models/transaction/createTransactionRequest.interface';
import { QueryParametersInterface } from '../../../core/models/common/queryParameters.interface';
import { SavingGoalService } from '../../../core/services/savingGoal.service';
import { SavingGoalResponse } from '../../../core/models/savingGoal/saving-goal-response.interface';
import { Type } from '../../../core/models/enums/type.enum';

@Component({
  selector: 'app-create-transaction-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './create-transaction-modal.html',
  styleUrl: './create-transaction-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateTransactionModal implements OnInit, OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private transactionService = inject(TransactionService);
  private categoryService = inject(CategoryService);
  private savingGoalService = inject(SavingGoalService);

  @Output() public created = new EventEmitter<void>();
  public isSubmitting = false;
  public isLoadingCategories = false;
  public isLoadingSavingGoals = false;
  public errorMessage = '';
  public categoriesError = '';
  public savingGoalsError = '';
  public showSuccessToast = false;
  public categories: CategoryResponse[] = [];
  public savingGoals: SavingGoalResponse[] = [];
  public readonly Type = Type;

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
    this.syncSavingGoalState();
  }

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onSubmit(): void {
    this.syncSavingGoalState();

    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const descriptionValue = rawValue.description.trim();
    const request: CreateTransactionRequest = {
      amount: Number(rawValue.amount),
      date: rawValue.date,
      description: descriptionValue.length > 0 ? descriptionValue : null,
      categoryId: Number(rawValue.categoryId),
      savingGoalId: this.isSavingCategorySelected() ? Number(rawValue.savingGoalId) : null,
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.transactionService.createTransaction(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.form.reset({
          date: '',
          amount: 0,
          categoryId: 0,
          savingGoalId: 0,
          description: '',
        });
        this.syncSavingGoalState();
        this.created.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo crear la transacción. Intenta nuevamente.';
      },
    });
  }

  public syncSavingGoalState(): void {
    if (!this.isSavingCategorySelected()) {
      this.form.controls.savingGoalId.setValue(0);
      this.form.controls.savingGoalId.clearValidators();
      this.form.controls.savingGoalId.updateValueAndValidity({ emitEvent: false });
      return;
    }

    this.form.controls.savingGoalId.setValidators([Validators.required, Validators.min(1)]);
    this.form.controls.savingGoalId.updateValueAndValidity({ emitEvent: false });
  }

  public isSavingCategorySelected(): boolean {
    const categoryId = Number(this.form.controls.categoryId.value);
    const selectedCategory = this.categories.find(category => category.id === categoryId);
    return selectedCategory?.type === Type.Saving;
  }

  public isSavingCategoryDisabled(category: CategoryResponse): boolean {
    return category.type === Type.Saving && this.savingGoals.length === 0;
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
