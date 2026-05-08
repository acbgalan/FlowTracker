import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SavingLogResponse } from '../models/savingLog/saving-log-response.interface';
import { CreateSavingLogRequest } from '../models/savingLog/create-saving-log-request.interface';
import { UpdateSavingLogRequest } from '../models/savingLog/update-saving-log-request.interface';

@Injectable({
  providedIn: 'root',
})
export class SavingLogService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/SavingLogs';

  public getSavingLogs(): Observable<SavingLogResponse[]> {
    return this.http.get<SavingLogResponse[]>(this.apiUrl);
  }

  public getSavingLog(id: number): Observable<SavingLogResponse> {
    return this.http.get<SavingLogResponse>(`${this.apiUrl}/${id}`);
  }

  public createSavingLog(request: CreateSavingLogRequest): Observable<SavingLogResponse> {
    return this.http.post<SavingLogResponse>(this.apiUrl, request);
  }

  public updateSavingLog(id: number, request: UpdateSavingLogRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  public deleteSavingLog(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
