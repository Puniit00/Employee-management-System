import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeGoalsService } from '../../services/employee-goals.service';
import { EmployeeGoal } from '../../employee.interface';

@Component({
  selector: 'app-add-goals',
  standalone: false,
  templateUrl: './add-goals.html',
  styleUrls: ['./add-goals.css']
})
export class AddGoalsComponent implements OnInit {
  public employeeId!: number;

  public goal = {
    title: '',
    description: '',
    targetDate: '',
    isCompleted: false
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private goalsService: EmployeeGoalsService,
  ) {}

  public ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
  }

  public onSubmit(form: any): void {
    if (form.valid) {
      const payload = {
        employeeId: this.employeeId,
        title: this.goal.title,
        description: this.goal.description,
        targetDate: this.goal.targetDate,
        isCompleted: this.goal.isCompleted,
        id: 0 // ID will be assigned by the backend
      };

      this.goalsService.addEmployeeGoal(payload).subscribe({
        next: () => {
          this.router.navigate(['/employee-goals', this.employeeId]);
        },
        error: (err) => {
          console.error('Error adding goal:', err);
        }
      });
    }
  }

  public cancel(): void {
    this.router.navigate(['/employee-goals', this.employeeId]);
  }
}
