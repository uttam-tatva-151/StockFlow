import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
})
export class RegisterComponent {
  private fb = inject(NonNullableFormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  // --- Reactive form with validators ---
  registerForm = this.fb.group({
    userName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
  });

  // --- Signals for UI state ---
  submitted = signal(false);
  errorMessage = signal<string | null>(null);

  // --- Getters using computed() ---
  userNameCtrl = computed(() => this.registerForm.controls.userName);
  emailCtrl = computed(() => this.registerForm.controls.email);
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

    const { userName, email, password } = this.registerForm.getRawValue();

    this.authService.register({ userName, email, password }).subscribe({
      next: () => this.router.navigate(['/auth/login']),
      error: (err) =>
        this.errorMessage.set(err.error?.message ?? 'Registration failed. Please try again.'),
    });
  }
}
