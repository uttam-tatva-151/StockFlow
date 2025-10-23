import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { EndPoints } from '../../../shared/constants/end-points';
import { SnackbarService } from '../../../shared/services/snackbar.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
})
export class RegisterComponent {

    private snackbar = inject(SnackbarService);
  private fb = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  // --- Reactive form with validators ---
  registerForm = this.fb.group({
    userName: ['', [Validators.required, Validators.minLength(3)]],
    emailId: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
  });

  // --- Signals for UI state ---
  submitted = signal(false);
  errorMessage = signal<string | null>(null);

  // --- Getters using computed() ---
  userNameCtrl = computed(() => this.registerForm.controls.userName);
  emailIdCtrl = computed(() => this.registerForm.controls.emailId);
  passwordCtrl = computed(() => this.registerForm.controls.password);
  confirmPasswordCtrl = computed(() => this.registerForm.controls.confirmPassword);

  // --- Helper to check password match ---
  private passwordsMatch(): boolean {
    const { password, confirmPassword } = this.registerForm.getRawValue();
    return password === confirmPassword;
  }

  // --- Submit handler ---
  onSubmit(): void {
    this.submitted.set(true);
    this.errorMessage.set(null);

    if (this.registerForm.invalid || !this.passwordsMatch()) {
      if (!this.passwordsMatch()) {
        this.errorMessage.set("Passwords don't match");
      }
      return;
    }

    const { userName, emailId, password } = this.registerForm.getRawValue();

    this.authService.register({ userName, emailId, password }).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackbar.success(response.message || 'Registration successful!');
        } else {
          this.snackbar.error(response.message || 'Something went wrong');
        }
        this.router.navigate([EndPoints.AUTH.LOGIN])

      },
      error: (err) =>{
        const msg = err.error?.message ?? 'Registration failed. Please try again.';
        this.snackbar.error(msg);}
    });
  }
}
