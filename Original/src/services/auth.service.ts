
import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';

export type UserRole = 'Receptionist' | 'Nurse' | 'Doctor' | 'Admin';

@Injectable({ providedIn: 'root' })
export class AuthService {
  isAuthenticated = signal(false);
  activeRole = signal<UserRole>('Receptionist');
  currentUser = signal({ name: 'Alex', username: 'alex_admin' });

  constructor(private router: Router) {}

  login(username: string, role: UserRole = 'Receptionist') {
    this.isAuthenticated.set(true);
    this.activeRole.set(role);
    
    let displayName = 'Alex';
    if (role === 'Doctor') displayName = 'Dr. Sarah';
    if (role === 'Admin') displayName = 'Alex (SysAdmin)';
    
    this.currentUser.set({ name: displayName, username });
    
    this.routeForRole(role);
  }

  switchRole(role: UserRole) {
    this.activeRole.set(role);
    this.routeForRole(role);
  }

  private routeForRole(role: UserRole) {
    switch (role) {
      case 'Doctor':
        this.router.navigate(['/app/doctor-dashboard']);
        break;
      case 'Admin':
        this.router.navigate(['/app/admin/dashboard']);
        break;
      default:
        this.router.navigate(['/app/dashboard']);
        break;
    }
  }

  logout() {
    this.isAuthenticated.set(false);
    this.router.navigate(['/']);
  }
}
