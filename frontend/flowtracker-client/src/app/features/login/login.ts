import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Login {
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    remember: [false],
  });


  onSubmit(): void {
    if (this.loginForm.invalid || this.isLoading()) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { remember, ...credentials } = this.loginForm.getRawValue() as any;

    this.userService.loginUser(credentials).subscribe({
      next: (response) => {
        // save token in localStorage (remember=true) or sessionStorage
        this.authService.saveToken(response.token, response.expiration, !!remember);
        this.isLoading.set(false);
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading.set(false);
        const errorMsg =
          error?.error?.message || 'Error al iniciar sesión. Intenta de nuevo.';
        this.errorMessage.set(errorMsg);
      },
    });
  }
}
