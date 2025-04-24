import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environment/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = environment.apiUrl + '/attendance';

  constructor(private http: HttpClient) { }

  toggleAttendance(): Observable<any> {
    return this.http.post(`${this.apiUrl}/toggle`, {});
  }

  getToday(): Observable<any> {
    return this.http.get(`${this.apiUrl}/today`);
  }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }

  exportAttendance(year: number, month: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/export?year=${year}&month=${month}`, { responseType: 'blob' });
  }

  getStatus(): Observable<any> {
    return this.http.get(`${this.apiUrl}/status`);
  }
}
