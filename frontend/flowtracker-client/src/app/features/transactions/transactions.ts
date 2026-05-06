import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransactionService } from '../../core/services/transaction.service';
import { TransactionResponse } from '../../core/models/transaction/transactionResponse.interface';
import { QueryParametersInterface } from '../../core/models/common/queryParameters.interface';
import { CreateTransactionModal } from './create-transaction-modal/create-transaction-modal';
import { EditTransactionModal } from './edit-transaction-modal/edit-transaction-modal';
import { DeleteTransactionModal } from './delete-transaction-modal/delete-transaction-modal';
import { SearchService } from '../../shared/services/search.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-transactions',
  imports: [
    CommonModule,
    CreateTransactionModal,
    EditTransactionModal,
    DeleteTransactionModal,
  ],
  templateUrl: './transactions.html',
  styleUrl: './transactions.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Transactions implements OnInit, OnDestroy {
  private transactionService = inject(TransactionService);
  private searchService = inject(SearchService);
  private cdr = inject(ChangeDetectorRef);
  private destroy$ = new Subject<void>();

  protected readonly Math = Math;

  public currentPage = 1;
  public itemsPerPage = 20;
  public totalItems = 0;

  public searchTerm: string | null = null;
  public sortBy: string | null = null;
  public sortDesc = false;

  public transactions: TransactionResponse[] = [];
  public isLoading = false;
  public errorMessage = '';
  public transactionToEdit: TransactionResponse | null = null;
  public transactionToDelete: TransactionResponse | null = null;

  public get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  public ngOnInit(): void {
    this.searchService.searchTerm$
      .pipe(takeUntil(this.destroy$))
      .subscribe(term => {
        this.searchTerm = term;
        this.currentPage = 1;
        this.getListData();
      });

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

    this.transactionService.getTransactions(queryParams).subscribe({
      next: response => {
        this.transactions = response.data;
        this.totalItems = response.total;
        this.isLoading = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.transactions = [];
        this.errorMessage = 'No se pudieron cargar las transacciones.';
        this.isLoading = false;
        this.cdr.markForCheck();
      },
    });
  }

  public toggleSort(field: string): void {
    if (this.sortBy === field) {
      this.sortDesc = !this.sortDesc;
    } else {
      this.sortBy = field;
      this.sortDesc = false;
    }
    this.currentPage = 1;
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

  public prepareDelete(transaction: TransactionResponse): void {
    this.transactionToDelete = { ...transaction };
  }

  public prepareEdit(transaction: TransactionResponse): void {
    this.transactionToEdit = { ...transaction };
  }

  public getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

}
