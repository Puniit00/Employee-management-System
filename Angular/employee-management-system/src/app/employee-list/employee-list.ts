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

  constructor(
    private employeeService: EmployeeService,
    private changeDetetctionStrategy: ChangeDetectorRef,
    private router: Router
  ) {
    this.employees = [];
  }

  public ngOnInit(): void {
    // Get employees on component initialization
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
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
}
