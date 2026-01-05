import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent {
    private authService = inject(AuthService);

    username = '';
    password = '';
    isLoading = false;
    errorMessage = '';

    onSubmit() {
        if (!this.username || !this.password) {
            this.errorMessage = 'Please enter both username and password';
            return;
        }

        this.isLoading = true;
        this.errorMessage = '';

        this.authService.login({ username: this.username, password: this.password })
            .subscribe({
                next: () => {
                    this.isLoading = false;
                    // Redirect handled in service
                },
                error: (err) => {
                    this.isLoading = false;
                    console.error(err);
                    this.errorMessage = err.error?.error || 'Login failed. Please check your credentials.';
                }
            });
    }
}
