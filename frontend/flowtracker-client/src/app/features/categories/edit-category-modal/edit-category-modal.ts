import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  OnChanges,
  SimpleChanges,
  inject,
  Input,
  Output,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryResponse } from '../../../core/models/category/categoryResponse.interface';
import { UpdateCategoryRequest } from '../../../core/models/category/updateCategoryRequest.interface';
import { Type } from '../../../core/models/enums/type.enum';
import { CategoryService } from '../../../core/services/category.service';

@Component({
  selector: 'app-edit-category-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-category-modal.html',
  styleUrl: './edit-category-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditCategoryModal implements OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private categoryService = inject(CategoryService);

  @Input() public category: CategoryResponse | null = null;
  @Output() public updated = new EventEmitter<void>();
  public isSubmitting = false;
  public errorMessage = '';
  public showSuccessToast = false;
  public readonly typeOptions = Object.values(Type);

  public readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    type: [Type.Expense, Validators.required],
    icon: [''],
    description: ['', [Validators.required, Validators.maxLength(300)]],
  });

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  constructor() {}

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['category']) {
      const selectedCategory = this.category;

      if (!selectedCategory) {
        this.form.reset({
          name: '',
          type: Type.Expense,
          icon: '',
          description: '',
        });
        this.errorMessage = '';
        this.isSubmitting = false;
        return;
      }

      this.form.reset({
        name: selectedCategory.name,
        type: selectedCategory.type,
        icon: selectedCategory.icon ?? '',
        description: selectedCategory.description,
      });
      this.errorMessage = '';
      this.isSubmitting = false;
    }
  }

  public onSubmit(): void {
    const selectedCategory = this.category;

    if (!selectedCategory || this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const iconValue = rawValue.icon.trim();
    const request: UpdateCategoryRequest = {
      id: selectedCategory.id,
      name: rawValue.name.trim(),
      type: rawValue.type,
      icon: iconValue.length > 0 ? iconValue : null,
      description: rawValue.description.trim(),
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.categoryService.updateCategory(selectedCategory.id, request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.updated.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo actualizar la categoría. Intenta nuevamente.';
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
