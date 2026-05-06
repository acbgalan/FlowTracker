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

  @Output() public created = new EventEmitter<void>();
  public isSubmitting = false;
  public isLoadingCategories = false;
  public errorMessage = '';
  public categoriesError = '';
  public showSuccessToast = false;
  public categories: CategoryResponse[] = [];

  public readonly form = this.formBuilder.nonNullable.group({
    date: ['', Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    description: ['', [Validators.maxLength(300)]],
  });

  public ngOnInit(): void {
    this.loadCategories();
  }

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onSubmit(): void {
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
          description: '',
        });
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
