import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryResponse } from '../models/category/categoryResponse.interface';
import { CreateCategoryRequest } from '../models/category/createCategoryRequest.interface';
import { UpdateCategoryRequestInterface } from '../models/category/updateCategoryRequest.interface';


@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/Categories';

  public getCategories(): Observable<CategoryResponse[]> {
    return this.http.get<CategoryResponse[]>(this.apiUrl);
  }

  public getCategory(id: number): Observable<CategoryResponse> {
    return this.http.get<CategoryResponse>(`${this.apiUrl}/${id}`);
  }

  public createCategory(request: CreateCategoryRequest): Observable<CategoryResponse> {
    return this.http.post<CategoryResponse>(this.apiUrl, request);
  }

  public updateCategory(id: number, request: UpdateCategoryRequestInterface):Observable<void>{
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  public deleteCategory(id: number): Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
