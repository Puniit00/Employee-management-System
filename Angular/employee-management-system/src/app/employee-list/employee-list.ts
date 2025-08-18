import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { EmployeeService } from '../services/employee.service';
import { Router } from '@angular/router';
import { Employee } from '../employee.interface';

@Component({
  selector: 'app-employee-list',
  standalone: false,
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployeeList implements OnInit {
  public employees: Employee[];
  public sortState: { [key: string]: 'asc' | 'desc' };
  public sortedEmployees: Employee[];

  constructor(
    private employeeService: EmployeeService,
    private changeDetetctionStrategy: ChangeDetectorRef,
    private router: Router
  ) {
    this.employees = [];
    this.sortState = {};
    this.sortedEmployees = [];
  }

  public ngOnInit(): void {
    // Get employees on component initialization
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
      this.sortedEmployees = [...this.employees]; // refresh sorted list
      this.changeDetetctionStrategy.markForCheck();
    });
  }

  public deleteEmployee(email: string): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      const id = this.employees.find(
        (employees: Employee) => employees.email.trim() === email.trim()
      )?.id;
      this.employeeService.deleteEmployee(id ?? 0).subscribe({
        next: () => {
          this.employees = this.employees.filter((e) => e.id !== id);
          this.sortedEmployees = [...this.employees]; // refresh sorted list
          this.changeDetetctionStrategy.detectChanges();
        },
        error: (err) => {
          console.error('Error deleting employee:', err);
        },
      });
    }
  }

  public onEdit(employee: Employee): void {
    this.router.navigate(['/update-employee'], {
      queryParams: { data: JSON.stringify(employee) },
    });
  }

  public sort(column: keyof Employee): void {
    const current = this.sortState[column];
    this.sortState = {}; // reset so only one column is active
    this.sortState[column] = current === 'asc' ? 'desc' : 'asc';

    this.sortedEmployees = [...this.employees].sort((a, b) => {
      let valA = a[column];
      let valB = b[column];

      if (typeof valA === 'string' && typeof valB === 'string') {
        valA = valA.toLowerCase();
        valB = valB.toLowerCase();
      }

      if (valA instanceof Date && valB instanceof Date) {
        valA = valA.getTime();
        valB = valB.getTime();
      }

      if (valA < valB) return this.sortState[column] === 'asc' ? -1 : 1;
      if (valA > valB) return this.sortState[column] === 'asc' ? 1 : -1;
      return 0;
    });

    this.changeDetetctionStrategy.markForCheck();
  }
}
