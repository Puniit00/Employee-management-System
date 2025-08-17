// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:44304/api'; 
  private tokenKey = 'jwtToken';

  constructor(private http: HttpClient) {}

  public login(username: string, password: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.baseUrl}/auth/login`, { username, password })
      .pipe(
        tap(response => {
          localStorage.setItem(this.tokenKey, response.token); 
        })
      );
  }

  public logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public isLoggedIn(): boolean {
    return !!this.getToken();
  }

  public signup(badgeNo: number,name: string, password: string): Observable<any> {
    return this.http.post(`https://localhost:44304/api/Auth/signup?badgeNo=${badgeNo}&name=${encodeURIComponent(name)}&password=${encodeURIComponent(password)}`, {});
  }
}
