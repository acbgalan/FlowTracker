import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryResponse } from '../models/category/categoryResponse.interface';
import { CreateCategoryRequest } from '../models/category/createCategoryRequest.interface';
import { UpdateCategoryRequest } from '../models/category/updateCategoryRequest.interface';
import { PagedCategoryResponse } from '../models/category/pagedCategoryResponse.interface';
import { QueryParametersInterface } from '../models/common/queryParameters.interface';


@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/Categories';

  public getCategories(queryParams: QueryParametersInterface): Observable<PagedCategoryResponse> {
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

    return this.http.get<PagedCategoryResponse>(this.apiUrl, { params });
  }

  public getCategory(id: number): Observable<CategoryResponse> {
    return this.http.get<CategoryResponse>(`${this.apiUrl}/${id}`);
  }

  public createCategory(request: CreateCategoryRequest): Observable<CategoryResponse> {
    return this.http.post<CategoryResponse>(this.apiUrl, request);
  }

  public updateCategory(id: number, request: UpdateCategoryRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  public deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
