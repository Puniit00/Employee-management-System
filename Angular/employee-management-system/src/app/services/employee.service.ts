import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../employee.interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:44304/api/Employees';

  constructor(private http: HttpClient) { }

  public getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  public getEmployee(id: number): Observable<any> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  public addEmployee(employee: Employee): Observable<any> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  public updateEmployee(id: number, employee: Employee): Observable<any> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  public deleteEmployee(id: number): Observable<Employee> {
    return this.http.delete<Employee>(`${this.apiUrl}/${id}`);
  }
}
