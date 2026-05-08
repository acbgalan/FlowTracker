import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SavingGoalResponse } from '../models/savingGoal/saving-goal-response.interface';
import { UpdateSavingGoalRequest } from '../models/savingGoal/update-saving-goal-request.interface';
import { CreateSavingGoalRequest } from '../models/savingGoal/create-saving-goal-request.interface';

@Injectable({
  providedIn: 'root',
})
export class SavingGoalService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/SavingGoals';

  public getSavingGoals(): Observable<SavingGoalResponse[]> {
    return this.http.get<SavingGoalResponse[]>(this.apiUrl);
  }

  public getSavingGoal(id: number): Observable<SavingGoalResponse> {
    return this.http.get<SavingGoalResponse>(`${this.apiUrl}/${id}`);
  }

  public createSavingGoal(request: CreateSavingGoalRequest): Observable<SavingGoalResponse> {
    return this.http.post<SavingGoalResponse>(this.apiUrl, request);
  }

  public updateSavingGoal(id: number, request: UpdateSavingGoalRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  public deleteSavingGoal(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
