import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
  OnDestroy,
  inject,
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
  private cdr = inject(ChangeDetectorRef);
  private destroy$ = new Subject<void>();

  // Math reference for template
  protected readonly Math = Math;

  // Pagination state
  public currentPage = 1;
  public itemsPerPage = 20;
  public totalItems = 0;
  
  // Search and filter state
  public searchTerm: string | null = null;
  public sortBy: string | null = null;
  public sortDesc = false;

  // Data state
  public categories: CategoryResponse[] = [];
  public isLoading = false;
  public errorMessage = '';
  public categoryToEdit: CategoryResponse | null = null;
  public categoryToDelete: CategoryResponse | null = null;

  // Total pages getter
  public get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  constructor() {}

  public ngOnInit(): void {
    // Subscribe to search events from header
    this.searchService.searchTerm$
      .pipe(takeUntil(this.destroy$))
      .subscribe(term => {
        this.searchTerm = term;
        this.currentPage = 1; // Reset to first page on new search
        this.getListData();
      });
    // Initial load
    this.getListData();
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public getListData(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.markForCheck();

    const queryParams: QueryParametersInterface = {
      searchTerm: this.searchTerm,
      sortBy: this.sortBy,
      sortDesc: this.sortDesc,
      page: this.currentPage,
      limit: this.itemsPerPage,
    };

    this.categoryService.getCategories(queryParams).subscribe({
      next: response => {
        this.categories = response.data;
        this.totalItems = response.total;
        this.isLoading = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.categories = [];
        this.errorMessage = 'No se pudieron cargar las categorías.';
        this.isLoading = false;
        this.cdr.markForCheck();
      },
    });
  }

  public toggleSort(field: string): void {
    if (this.sortBy === field) {
      // Toggle sort direction if clicking the same field
      this.sortDesc = !this.sortDesc;
    } else {
      // Set new field and ascending order
      this.sortBy = field;
      this.sortDesc = false;
    }
    this.currentPage = 1; // Reset to first page when sorting changes
    this.getListData();
  }

  public goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.getListData();
    }
  }

  public prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage = this.currentPage - 1;
      this.getListData();
    }
  }

  public nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage = this.currentPage + 1;
      this.getListData();
    }
  }

  public prepareDelete(category: CategoryResponse): void {
    this.categoryToDelete = { ...category };
  }

  public prepareEdit(category: CategoryResponse): void {
    this.categoryToEdit = { ...category };
  }

  public getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }
}
