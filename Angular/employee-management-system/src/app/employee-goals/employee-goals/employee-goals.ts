import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeGoalsService } from '../../services/employee-goals.service';
import { EmployeeGoal } from '../../employee.interface';

@Component({
  selector: 'app-employee-goals',
  standalone: false,
  templateUrl: './employee-goals.html',
  styleUrls: ['./employee-goals.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployeeGoalsComponent implements OnInit {
  public employeeId!: number;
  public goals: any[] = [];
  public editingIndex: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private goalsService: EmployeeGoalsService,
    private cdr: ChangeDetectorRef
  ) {}

  public ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.employeeId) {
      this.loadGoals();
    }
  }

  public loadGoals(): void {
    this.goalsService.getEmployeeGoals(this.employeeId).subscribe({
      next: (data) => {
        for (let i = 0; i < data.length; i++) {
          if (data[i].targetDate) {
            const dateObj = new Date(data[i].targetDate);
            data[i].targetDate = dateObj.toISOString().split('T')[0]; // 'YYYY-MM-DD'
          }
        }
        this.goals = data;
        this.cdr.markForCheck();
      },
      error: (err) => console.error('Error loading employee goals:', err),
    });
  }

  public addGoal(): void {
    this.router.navigate(['/add-goal', this.employeeId]);
  }

  public editGoal(index: number): void {
    this.editingIndex = index;
    this.cdr.markForCheck();
  }

  public cancelEdit(): void {
    this.editingIndex = null;
    this.loadGoals(); // reload to discard changes
  }

  public saveGoal(goal: any, idx: number): void {
    const payload: EmployeeGoal = {
      employeeId: this.employeeId,
      title: goal.title,
      description: goal.description,
      targetDate: goal.targetDate,
      isCompleted: goal.isCompleted,
      id: this.goals[idx].id,
    };
    this.goalsService
      .updateEmployeeGoal(this.goals[idx].id, payload)
      .subscribe({
        next: () => {
          this.editingIndex = null;
          this.loadGoals();
        },
        error: (err) => console.error('Error updating goal:', err),
      });
  }
}
