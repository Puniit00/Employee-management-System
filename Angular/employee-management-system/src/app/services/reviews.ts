import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PerformanceReview } from '../employee.interface';

@Injectable({
  providedIn: 'root'
})
export class Reviews {
  private baseUrl = 'https://localhost:44304/';
  constructor(private httpClient: HttpClient) { }

  public getPerformanceReview(employeeId: number) {
    return this.httpClient.get<PerformanceReview[]>(`${this.baseUrl}GetPerformanceReviews/${employeeId}`);
  }
}
