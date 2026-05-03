import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  effect,
  inject,
  input,
  output,
  signal,
  viewChild,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryResponse } from '../../../core/models/category/categoryResponse.interface';
import { UpdateCategoryRequestInterface } from '../../../core/models/category/updateCategoryRequest.interface';
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
  private dismissButton = viewChild<ElementRef<HTMLButtonElement>>('dismissButton');
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private categoryService = inject(CategoryService);

  public readonly category = input<CategoryResponse | null>(null);
  public readonly updated = output<void>();
  public readonly isSubmitting = signal(false);
  public readonly errorMessage = signal('');
  public readonly showSuccessToast = signal(false);
  public readonly typeOptions = Object.values(Type);

  public readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    type: [Type.Expense, Validators.required],
    icon: [''],
    description: ['', [Validators.required, Validators.maxLength(300)]],
  });

  constructor() {
    effect(
      () => {
        const selectedCategory = this.category();

        if (!selectedCategory) {
          this.form.reset({
            name: '',
            type: Type.Expense,
            icon: '',
            description: '',
          });
          this.errorMessage.set('');
          this.isSubmitting.set(false);
          return;
        }

        this.form.reset({
          name: selectedCategory.name,
          type: selectedCategory.type,
          icon: selectedCategory.icon ?? '',
          description: selectedCategory.description,
        });
        this.errorMessage.set('');
        this.isSubmitting.set(false);
      },
      { allowSignalWrites: true },
    );
  }

  public onSubmit(): void {
    const selectedCategory = this.category();

    if (!selectedCategory || this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const iconValue = rawValue.icon.trim();
    const request: UpdateCategoryRequestInterface = {
      id: selectedCategory.id,
      name: rawValue.name.trim(),
      type: rawValue.type,
      icon: iconValue.length > 0 ? iconValue : null,
      description: rawValue.description.trim(),
    };

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.categoryService.updateCategory(selectedCategory.id, request).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.updated.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.errorMessage.set('No se pudo actualizar la categoría. Intenta nuevamente.');
      },
    });
  }

  private hideModal(): void {
    this.dismissButton()?.nativeElement.click();
  }

  private showSuccessToastMessage(): void {
    this.showSuccessToast.set(true);

    if (this.toastTimeoutId !== null) {
      window.clearTimeout(this.toastTimeoutId);
    }

    this.toastTimeoutId = window.setTimeout(() => {
      this.showSuccessToast.set(false);
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
