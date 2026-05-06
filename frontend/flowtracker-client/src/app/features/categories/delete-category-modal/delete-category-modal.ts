import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  inject,
  Input,
  Output,
  EventEmitter,
  ViewChild,
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
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private categoryService = inject(CategoryService);

  @Input() public category: CategoryResponse | null = null;
  @Output() public deleted = new EventEmitter<void>();
  public isDeleting = false;
  public errorMessage = '';
  public showSuccessToast = false;

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onDelete(): void {
    const selectedCategory = this.category;

    if (!selectedCategory || this.isDeleting) {
      return;
    }

    this.isDeleting = true;
    this.errorMessage = '';

    this.categoryService.deleteCategory(selectedCategory.id).subscribe({
      next: () => {
        this.isDeleting = false;
        this.deleted.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'No se pudo eliminar la categoría. Intenta nuevamente.';
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
