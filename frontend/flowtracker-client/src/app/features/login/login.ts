import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Login {
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  isLoading = false;
  errorMessage: string | null = null;

  dismissError(): void {
    this.errorMessage = null;
  }

  get emailControl() {
    return this.loginForm.controls.email;
  }

  get passwordControl() {
    return this.loginForm.controls.password;
  }

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    remember: [false],
  });


  onSubmit(): void {
    if (this.loginForm.invalid || this.isLoading) return;

    this.isLoading = true;
    this.errorMessage = null;

    const { remember, ...credentials } = this.loginForm.getRawValue();

    this.userService.loginUser(credentials).subscribe({
      next: (response) => {
        // save token in localStorage (remember=true) or sessionStorage
        this.authService.saveToken(response.token, response.expiration, !!remember);
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.isLoading = false;
        const errorMsg =
          error?.error?.message || 'Error al iniciar sesión. Intenta de nuevo.';
        this.errorMessage = errorMsg;
      },
    });
  }
}
