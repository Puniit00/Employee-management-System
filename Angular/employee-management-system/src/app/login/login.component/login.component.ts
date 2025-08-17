import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false // ✅ Ensure this is set to false if you are not using standalone components  
})
export class LoginComponent {
  public username: string;
  public password: string;

  constructor(private authService: AuthService, private router: Router) {
    this.username = '';
    this.password = '';
  }

  public onLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        alert('✅ Login successful');
        this.router.navigate(['/employee']); // Navigate to employee list on successful login
      },
      error: (err) => {
        alert('❌ Login failed');
        console.error(err);
      }
    });
  }
}
