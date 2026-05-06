import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TransactionResponse } from '../models/transaction/transactionResponse.interface';
import { CreateTransactionRequest } from '../models/transaction/createTransactionRequest.interface';
import { UpdateTransactionRequest } from '../models/transaction/updateTransactionRequest.interface';
import { PagedTransactionResponse } from '../models/transaction/pagedTransactionResponse.interface';
import { QueryParametersInterface } from '../models/common/queryParameters.interface';


@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/Transactions';

  public getTransactions(queryParams: QueryParametersInterface): Observable<PagedTransactionResponse> {
    let params = new HttpParams();

    if (queryParams.searchTerm) {
      params = params.set('searchTerm', queryParams.searchTerm);
    }
    if (queryParams.sortBy) {
      params = params.set('sortBy', queryParams.sortBy);
    }
    params = params.set('sortDesc', queryParams.sortDesc.toString());
    params = params.set('page', queryParams.page.toString());
    params = params.set('limit', queryParams.limit.toString());

    return this.http.get<PagedTransactionResponse>(this.apiUrl, { params });
  }

  public getTransaction(id: number): Observable<TransactionResponse> {
    return this.http.get<TransactionResponse>(`${this.apiUrl}/${id}`);
  }

  public createTransaction(request: CreateTransactionRequest): Observable<TransactionResponse> {
    return this.http.post<TransactionResponse>(this.apiUrl, request);
  }

  public updateTransaction(id: number, request: UpdateTransactionRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  public deleteTransaction(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
