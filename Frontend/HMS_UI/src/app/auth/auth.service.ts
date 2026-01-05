import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { catchError, tap, throwError, Observable } from 'rxjs';

export interface LoginResponse {
    accessToken: string;
    expiresIn: number;
    issuedAt: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private router = inject(Router);
    private apiUrl = environment.apiUrl;

    // Signal to track login state (optional but good practice)
    currentUser = signal<any>(null);

    constructor() {
        const token = localStorage.getItem('access_token');
        if (token) {
            const payload = this.decodeToken(token);
            this.currentUser.set(payload);
        }
    }

    // Mock data from DB/seed.sql
    private mockUsers: { [key: string]: string } = {
        'admin1': 'Admin',
        'doctor1': 'Doctor',
        'nurse1': 'Nurse',
        'receptionist1': 'Receptionist',
        'billingclerk1': 'BillingClerk',
        'patient1': 'Patient' // Added for frontend testing
    };

    login(credentials: { username: string; password: string }) {
        // Mock Login Logic
        if (this.mockUsers[credentials.username]) {
            console.warn('Using MOCK login for testing');
            const role = this.mockUsers[credentials.username];
            const mockToken = this.createMockToken(role, credentials.username);

            const mockResponse: LoginResponse = {
                accessToken: mockToken,
                expiresIn: 3600,
                issuedAt: new Date().toISOString()
            };

            // Return an observable that emits the mock response
            return new Observable<LoginResponse>(observer => {
                this.setSession(mockResponse);
                observer.next(mockResponse);
                observer.complete();
            });
        }

        // Real API Call
        return this.http.post<LoginResponse>(`${this.apiUrl}/api/auth/login`, credentials).pipe(
            tap(response => {
                this.setSession(response);
            }),
            catchError(error => {
                return throwError(() => error);
            })
        );
    }

    logout() {
        // Call backend logout if needed, remove token, redirect
        // For mock, just clear local state
        if (!this.currentUser()?.isMock) {
            this.http.post(`${this.apiUrl}/api/auth/logout`, {}).subscribe({
                error: () => { /* Ignore errors on logout */ }
            });
        }

        localStorage.removeItem('access_token');
        this.currentUser.set(null);
        this.router.navigate(['/login']);
    }

    private setSession(authResult: LoginResponse) {
        localStorage.setItem('access_token', authResult.accessToken);
        const payload = this.decodeToken(authResult.accessToken);
        this.currentUser.set(payload);

        // Redirect based on role
        const role = payload.role;
        this.redirectUser(role);
    }

    private redirectUser(role: string) {
        switch (role?.toLowerCase()) {
            case 'admin':
                this.router.navigate(['/dashboard/admin']);
                break;
            case 'doctor':
                this.router.navigate(['/dashboard/doctor']);
                break;
            case 'nurse':
                this.router.navigate(['/dashboard/nurse']);
                break;
            case 'receptionist':
                this.router.navigate(['/dashboard/receptionist']);
                break;
            case 'billingclerk':
                this.router.navigate(['/dashboard/billing']);
                break;
            case 'patient':
                this.router.navigate(['/dashboard/patient']);
                break;
            default:
                this.router.navigate(['/']); // Fallback
                break;
        }
    }

    private decodeToken(token: string): any {
        try {
            return JSON.parse(atob(token.split('.')[1]));
        } catch (e) {
            console.error('Error decoding token', e);
            return null;
        }
    }

    private createMockToken(role: string, username: string): string {
        const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
        const payload = btoa(JSON.stringify({
            sub: 'mock-id',
            name: username,
            role: role,
            isMock: true
        }));
        const signature = 'mock-signature';
        return `${header}.${payload}.${signature}`;
    }
}
