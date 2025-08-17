import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {
  public isLoggedIn : boolean = false;

  constructor(private router: Router, private cd: ChangeDetectorRef) {}

  public ngOnInit(): void {
    // Check login state on init
    this.isLoggedIn = !!localStorage.getItem('jwtToken');
  }

  public logout(): void {
    localStorage.removeItem('jwtToken'); // remove token
    this.isLoggedIn = false;
    this.router.navigate(['']); // redirect to login
    this.cd.detectChanges();
  }
}
