import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MonthlySummaryResponse } from '../models/dashboard/monthlySummaryResponse.interface';
import { MonthlyExpenseByCategory } from '../models/dashboard/monthlyExpenseByCategory.interface';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/Dashboard';

  public getMonthlySummary(date: string): Observable<MonthlySummaryResponse> {
    let params = new HttpParams().set('date', date);
    return this.http.get<MonthlySummaryResponse>(`${this.apiUrl}/monthly-summary`, { params });
  }

  public getMonthlyExpensesByCategory(date: string): Observable<MonthlyExpenseByCategory[]> {
    let params = new HttpParams().set('date', date);
    return this.http.get<MonthlyExpenseByCategory[]>(`${this.apiUrl}/monthly-expenses-by-category`, { params });
  }
}
