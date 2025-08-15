import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployeeGoal } from '../employee.interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeGoalsService {
  private baseApiUrl = 'https://localhost:44304/';

  constructor(private httpClient: HttpClient) {}

  /** GET: Get all goals for an employee */
  getEmployeeGoals(employeeId: number): Observable<EmployeeGoal[]> {
    return this.httpClient.get<EmployeeGoal[]>(
      `${this.baseApiUrl}GetEmployeeGoals/${employeeId}`
    );
  }

  /** POST: Add a new employee goal */
  addEmployeeGoal(goal: EmployeeGoal): Observable<any> {
    return this.httpClient.post<any>(
      `${this.baseApiUrl}AddEmployeeGoals`,
      goal
    );
  }

  /** PUT: Update an existing employee goal by ID */
  updateEmployeeGoal(id: number, goal: EmployeeGoal): Observable<any> {
    return this.httpClient.put<any>(
      `${this.baseApiUrl}UpdateEmployeeGoals/${id}`,
      goal
    );
  }
}
