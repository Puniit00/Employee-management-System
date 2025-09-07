import { CommonModule } from '@angular/common';
import { Component, ChangeDetectionStrategy, ChangeDetectorRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CdkVirtualScrollViewport, ScrollingModule } from '@angular/cdk/scrolling';
import { EmployeeService } from '../services/employee.service';
import { Employee } from '../employee.interface';
import { debounceTime, distinctUntilChanged, of, Subject, switchMap } from 'rxjs';

@Component({
  selector: 'app-employee-list',
  standalone: true, // ✅ standalone
  imports: [CommonModule, ScrollingModule], // ✅ import modules you need
  templateUrl: './employee-list.html',
  styleUrls: ['./employee-list.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployeeList implements OnInit {
  public employees: Employee[] = [];
  public sortState: { [key: string]: 'asc' | 'desc' } = {};
  public sortedEmployees: Employee[] = [];
  public nameFilter: string = '';
  public rowHeight = 48;
  private filterSubject = new Subject<string>();

  @ViewChild(CdkVirtualScrollViewport) viewport!: CdkVirtualScrollViewport;

  constructor(
    private employeeService: EmployeeService,
    private changeDetetctionStrategy: ChangeDetectorRef,
    private router: Router
  ) {}

  get viewportHeight(): number {
    const rows = this.sortedEmployees.length;
    return Math.min(rows * this.rowHeight, 1000);
  }

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
      this.sortedEmployees = [...this.employees];

      this.filterSubject
        .pipe(
          debounceTime(300),
          distinctUntilChanged(),
          switchMap((value) => {
            const filterValue = value.trim().toLowerCase();
            return of(this.employees.filter((emp) => emp.name.toLowerCase().includes(filterValue)));
          })
        )
        .subscribe((filtered) => {
          this.sortedEmployees = filtered;
          this.changeDetetctionStrategy.markForCheck();
        });

      this.changeDetetctionStrategy.markForCheck();
    });
  }

  deleteEmployee(email: string): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      const id = this.employees.find((emp) => emp.email.trim() === email.trim())?.id;
      this.employeeService.deleteEmployee(id ?? 0).subscribe({
        next: () => {
          this.employees = this.employees.filter((e) => e.id !== id);
          this.sortedEmployees = [...this.employees];
          this.changeDetetctionStrategy.detectChanges();
        },
        error: (err) => console.error('Error deleting employee:', err),
      });
    }
  }

  onEdit(employee: Employee): void {
    this.router.navigate(['/update-employee'], {
      queryParams: { data: JSON.stringify(employee) },
    });
  }

  sort(column: keyof Employee): void {
    const current = this.sortState[column];
    this.sortState = {};
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

  applyFilter(): void {
    this.filterSubject.next(this.nameFilter);
  }

  trackByEmployeeId(index: number, employee: Employee): number {
    return employee.id;
  }
}
