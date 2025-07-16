import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class RegisterComponent {
  registerForm: FormGroup;
  error = '';
  success = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  register() {
    this.error = '';
    this.success = '';
    if (this.registerForm.invalid) return;
    const { username, password } = this.registerForm.value;
    this.authService.register(username, password).subscribe({
      next: () => {
        this.success = 'User registered successfully!';
        this.registerForm.reset();
      },
      error: (err) => {
        this.error = err.error?.message || 'Registration failed.';
      }
    });
  }
} 