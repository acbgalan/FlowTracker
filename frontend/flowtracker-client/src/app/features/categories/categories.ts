import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  OnDestroy,
  inject,
  signal,
  computed,
  effect,
} from '@angular/core';
import { CategoryService } from '../../core/services/category.service';
import { CategoryResponse } from '../../core/models/category/categoryResponse.interface';
import { QueryParametersInterface } from '../../core/models/common/queryParameters.interface';
import { CreateCategoryModal } from './create-category-modal/create-category-modal';
import { DeleteCategoryModal } from './delete-category-modal/delete-category-modal';
import { EditCategoryModal } from './edit-category-modal/edit-category-modal';
import { SearchService } from '../../shared/services/search.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-categories',
  imports: [CreateCategoryModal, DeleteCategoryModal, EditCategoryModal],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Categories implements OnInit, OnDestroy {
  private categoryService = inject(CategoryService);
  private searchService = inject(SearchService);
  private destroy$ = new Subject<void>();

  // Math reference for template
  protected readonly Math = Math;

  // Pagination signals
  public readonly currentPage = signal(1);
  public readonly itemsPerPage = signal(20);
  public readonly totalItems = signal(0);
  
  // Search and filter signals
  public readonly searchTerm = signal<string | null>(null);
  public readonly sortBy = signal<string | null>(null);
  public readonly sortDesc = signal(false);

  // Data signals
  public categories = signal<CategoryResponse[]>([]);
  public readonly isLoading = signal(false);
  public readonly errorMessage = signal('');
  public readonly categoryToEdit = signal<CategoryResponse | null>(null);
  public readonly categoryToDelete = signal<CategoryResponse | null>(null);

  // Computed total pages
  public totalPages = computed(() => Math.ceil(this.totalItems() / this.itemsPerPage()));

  constructor() {
    // Effect to load data when any filter/page changes
    effect(() => {
      this.getListData();
    });
  }

  public ngOnInit(): void {
    // Subscribe to search events from header
    this.searchService.searchTerm$
      .pipe(takeUntil(this.destroy$))
      .subscribe(term => {
        this.searchTerm.set(term);
        this.currentPage.set(1); // Reset to first page on new search
      });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public getListData(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    const queryParams: QueryParametersInterface = {
      searchTerm: this.searchTerm(),
      sortBy: this.sortBy(),
      sortDesc: this.sortDesc(),
      page: this.currentPage(),
      limit: this.itemsPerPage(),
    };

    this.categoryService.getCategories(queryParams).subscribe({
      next: response => {
        this.categories.set(response.data);
        this.totalItems.set(response.total);
        this.isLoading.set(false);
      },
      error: () => {
        this.categories.set([]);
        this.errorMessage.set('No se pudieron cargar las categorías.');
        this.isLoading.set(false);
      },
    });
  }

  public toggleSort(field: string): void {
    if (this.sortBy() === field) {
      // Toggle sort direction if clicking the same field
      this.sortDesc.set(!this.sortDesc());
    } else {
      // Set new field and ascending order
      this.sortBy.set(field);
      this.sortDesc.set(false);
    }
    this.currentPage.set(1); // Reset to first page when sorting changes
  }

  public goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  public prevPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.set(this.currentPage() - 1);
    }
  }

  public nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.set(this.currentPage() + 1);
    }
  }

  public prepareDelete(category: CategoryResponse): void {
    this.categoryToDelete.set({ ...category });
  }

  public prepareEdit(category: CategoryResponse): void {
    this.categoryToEdit.set({ ...category });
  }
}
