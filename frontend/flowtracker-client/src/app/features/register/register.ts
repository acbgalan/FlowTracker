import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';

const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const form = control as any;
  const password = form.get('password')?.value;
  const confirmPassword = form.get('confirmPassword')?.value;

  return password && confirmPassword && password !== confirmPassword
    ? { passwordMismatch: true }
    : null;
};

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Register {
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  dismissError(): void {
    this.errorMessage = null;
  }

  get firstNameControl() {
    return this.registerForm.controls.firstName;
  }

  get emailControl() {
    return this.registerForm.controls.email;
  }

  get passwordControl() {
    return this.registerForm.controls.password;
  }

  get confirmPasswordControl() {
    return this.registerForm.controls.confirmPassword;
  }

  get termsControl() {
    return this.registerForm.controls.terms;
  }

  registerForm = this.fb.nonNullable.group(
    {
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      terms: [false, [Validators.requiredTrue]],
    },
    { validators: passwordMatchValidator }
  );

  onSubmit(): void {
    if (this.registerForm.invalid || this.isLoading) return;

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    const formValue = this.registerForm.getRawValue();
    const registerRequest = {
      firstName: formValue.firstName,
      lastName: formValue.lastName || null,
      email: formValue.email,
      password: formValue.password,
    };

    this.userService.registerUser(registerRequest).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token, response.expiration);
        this.isLoading = false;
        this.successMessage = 'Cuenta creada exitosamente. Redirigiendo...';
        setTimeout(() => {
          this.router.navigate(['/home']);
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;
        const errorMsg =
          error?.error?.message || 'Error al crear la cuenta. Intenta de nuevo.';
        this.errorMessage = errorMsg;
      },
    });
  }
}
