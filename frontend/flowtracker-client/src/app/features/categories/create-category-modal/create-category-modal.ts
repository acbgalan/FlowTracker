import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  ViewChild,
  inject,
  Output,
  EventEmitter,
} from '@angular/core';
import { ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { CategoryService } from '../../../core/services/category.service';
import { CreateCategoryRequest } from '../../../core/models/category/createCategoryRequest.interface';
import { Type } from '../../../core/models/enums/type.enum';

@Component({
  selector: 'app-create-category-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './create-category-modal.html',
  styleUrl: './create-category-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCategoryModal implements OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private formBuilder = inject(FormBuilder);
  private categoryService = inject(CategoryService);

  @Output() public created = new EventEmitter<void>();
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

  public onSubmit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const rawValue = this.form.getRawValue();
    const iconValue = rawValue.icon.trim();
    const request: CreateCategoryRequest = {
      name: rawValue.name.trim(),
      type: rawValue.type,
      icon: iconValue.length > 0 ? iconValue : null,
      description: rawValue.description.trim(),
    };

    this.isSubmitting = true;
    this.errorMessage = '';

    this.categoryService.createCategory(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.form.reset({
          name: '',
          type: Type.Expense,
          icon: '',
          description: '',
        });
        this.created.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'No se pudo crear la categoría. Intenta nuevamente.';
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
