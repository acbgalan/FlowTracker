import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CategoryService } from '../../core/services/category.service';
import { CategoryResponse } from '../../core/models/category/categoryResponse.interface';
import { CreateCategoryModal } from './create-category-modal/create-category-modal';
import { DeleteCategoryModal } from './delete-category-modal/delete-category-modal';
import { EditCategoryModal } from './edit-category-modal/edit-category-modal';


@Component({
  selector: 'app-categories',
  imports: [CreateCategoryModal, DeleteCategoryModal, EditCategoryModal],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Categories implements OnInit {
  private categoryService = inject(CategoryService);
  private cdr = inject(ChangeDetectorRef);

  public categories: CategoryResponse[] = [];
  public readonly isLoading = signal(false);
  public readonly errorMessage = signal('');
  public readonly categoryToEdit = signal<CategoryResponse | null>(null);
  public readonly categoryToDelete = signal<CategoryResponse | null>(null);

  public ngOnInit(): void {
    this.getListData();
  }

  public getListData(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.categoryService.getCategories().subscribe({
      next: response => {
        this.categories = response;
        this.isLoading.set(false);
        this.cdr.markForCheck();
      },
      error: () => {
        this.categories = [];
        this.errorMessage.set('No se pudieron cargar las categorías.');
        this.isLoading.set(false);
        this.cdr.markForCheck();
      },
    });
  }

  public prepareDelete(category: CategoryResponse): void {
    this.categoryToDelete.set({ ...category });
  }

  public prepareEdit(category: CategoryResponse): void {
    this.categoryToEdit.set({ ...category });
  }
}
