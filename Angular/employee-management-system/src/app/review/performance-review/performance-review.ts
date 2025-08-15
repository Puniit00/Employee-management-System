import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { Reviews } from '../../services/reviews';
import { ActivatedRoute } from '@angular/router';
import { PerformanceReview } from '../../employee.interface';

@Component({
  selector: 'app-performance-review',
  standalone: false,
  templateUrl: './performance-review.html',
  styleUrl: './performance-review.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PerformanceReviewComponent implements OnInit {
  public employeeId!: number;
  public performanceReviews: PerformanceReview[];
  public isLoading: boolean;

  constructor(
    private reviewService: Reviews,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef
  ) {
    this.employeeId = this.route.snapshot.params['id'];
    this.performanceReviews = [];
    this.isLoading = true;
  }

  public ngOnInit(): void {
    if (this.performanceReviews.length === 0) {
      this.getPerformanceReviews();
    }
  }

  public getPerformanceReviews(): void {
    this.reviewService.getPerformanceReview(this.employeeId).subscribe({
      next: (data: PerformanceReview[]) => {
        this.performanceReviews = data;
        this.isLoading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching performance reviews:', err);
        this.isLoading = false;
        this.cd.detectChanges();
      },
    });
  }
}
