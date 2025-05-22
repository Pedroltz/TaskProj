import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { RegisterDto } from '../../../models/register-dto';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: RegisterDto = { name: '', email: '', password: '' };
  error = '';
  success = false;

  constructor(private auth: AuthService, private router: Router) {}

  onSubmit() {
    this.error = '';
    this.success = false;

    this.auth.register(this.user).subscribe({
      next: () => this.success = true,
      error: err => this.error = err.error?.title || 'Falha no registro'
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
