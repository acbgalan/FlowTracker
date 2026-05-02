import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  inject,
  input,
  output,
  signal,
  viewChild,
} from '@angular/core';
import { CategoryResponse } from '../../../core/models/category/categoryResponse.interface';
import { CategoryService } from '../../../core/services/category.service';

@Component({
  selector: 'app-delete-category-modal',
  imports: [],
  templateUrl: './delete-category-modal.html',
  styleUrl: './delete-category-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteCategoryModal implements OnDestroy {
  private dismissButton = viewChild<ElementRef<HTMLButtonElement>>('dismissButton');
  private toastTimeoutId: number | null = null;

  private categoryService = inject(CategoryService);

  public readonly category = input<CategoryResponse | null>(null);
  public readonly deleted = output<void>();
  public readonly isDeleting = signal(false);
  public readonly errorMessage = signal('');
  public readonly showSuccessToast = signal(false);

  public onDelete(): void {
    const selectedCategory = this.category();

    if (!selectedCategory || this.isDeleting()) {
      return;
    }

    this.isDeleting.set(true);
    this.errorMessage.set('');

    this.categoryService.deleteCategory(selectedCategory.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.deleted.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isDeleting.set(false);
        this.errorMessage.set('No se pudo eliminar la categoría. Intenta nuevamente.');
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
