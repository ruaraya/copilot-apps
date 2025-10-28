import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../api/api.service';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, MatCardModule, MatButtonModule, MatFormFieldModule, MatInputModule, NgIf],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private api: ApiService, private router: Router) {}

  login(): void {

    this.api.post<{ token: string }>('/auth/login', { username: this.username, password: this.password });

    if (!this.username || !this.password) {
      this.error = 'Username and password required';
      return;
    } 
    
    this.api.post<{ token: string }>('/auth/login', { username: this.username, password: this.password })
      .subscribe({
        next: (res: { token: string; }) => {
          localStorage.setItem('token', res.token);
          this.router.navigate(['/leads']);
        },
        error: () => this.error = 'Invalid credentials'
      });
  }

}