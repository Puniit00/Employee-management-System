import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
} from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  standalone: false, // This is important if you are using this component in a module,
  changeDetection: ChangeDetectionStrategy.OnPush, // Optional, for performance optimization
})
export class SignupComponent {
  public badgeNo: number;
  public username: string;
  public password: string;
  public confirmPassword: string;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cd: ChangeDetectorRef
  ) {
    this.badgeNo = 0;
    this.username = '';
    this.password = '';
    this.confirmPassword = '';
  }

  public signup(): void {
    if (this.password !== this.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }

    this.authService
      .signup(this.badgeNo, this.username, this.password)
      .subscribe({
        next: () => {
          alert('Signup successful! Please login.');
          this.router.navigate(['']);
          this.cd.markForCheck(); // Ensure the view updates
        },
        error: (err) => {
          console.error(err);
          alert('Signup failed. Try again.');
          this.cd.markForCheck(); // Ensure the view updates
        },
      });
  }
}
