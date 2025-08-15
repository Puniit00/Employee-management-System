import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeService } from '../services/employee.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.html',
  standalone: false
})
export class AddEmployeeComponent implements OnInit {
  public employeeForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.employeeForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      department: ['', Validators.required],
      dateOfJoining: ['', Validators.required]
    });
  }

  public addEmployee(): void {
    if (this.employeeForm.valid) {
      this.employeeService.addEmployee(this.employeeForm.value).subscribe({
        next: (response) => {
          console.log('Employee added successfully:', response);
          alert('Employee has been added successfully!');
          this.router.navigate(['/']); // Navigate back to employee list
          this.employeeForm.reset();
        },
        error: (error) => {
          console.error('Error adding employee:', error);
        }
      });
    } else {
      this.employeeForm.markAllAsTouched();
    }
  }
}
