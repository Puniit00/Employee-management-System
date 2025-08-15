import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../services/employee.service';

@Component({
  selector: 'app-update-employee',
  templateUrl: './update-employee.html',
  styleUrls: ['./update-employee.css'],
  standalone: false,
})
export class UpdateEmployeeComponent implements OnInit {
  public employeeForm!: FormGroup;
  private id!: number

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService
  ) {}

  public ngOnInit(): void {
    const data = this.route.snapshot.queryParamMap.get('data');
    let employee = null;
    if (data) {
      try {
        employee = JSON.parse(data);
      } catch (err) {
        console.error('Error parsing employee data', err);
      }
    }
      let formattedDate = '';
  if (employee?.dateOfJoining) {
    const dateObj = new Date(employee.dateOfJoining);
    formattedDate = dateObj.toISOString().split('T')[0]; // 'YYYY-MM-DD'
  }
    this.id = employee ? employee.id : null
    this.employeeForm = this.fb.group({
      name: [employee?.name || ''],
      email: [employee?.email || ''],
      department: [employee?.department || ''],
      dateOfJoining: [formattedDate]
    });
  }

  public updateEmployee(): void {
    if (this.employeeForm.valid) {
    this.employeeService.updateEmployee(this.id, this.employeeForm.value).subscribe({
      next: () => {
        alert('Employee data has been updated successfully!');
        this.router.navigate(['/']); // Go back to home page
      },
      error: (err) => {
        console.error('Error updating employee:', err);
        alert('Failed to update employee data.');
      }
    });
    }
  }

  public cancel(): void {
    this.router.navigate(['/']); // Home
  }
}
