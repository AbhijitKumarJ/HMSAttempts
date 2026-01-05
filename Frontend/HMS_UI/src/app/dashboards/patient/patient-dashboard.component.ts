import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  template: `
    <div style="padding: 2rem;">
      <h1>Patient Dashboard</h1>
      <p>Welcome. View your medical history and appointments.</p>
      <button (click)="logout()">Logout</button>
    </div>
  `
})
export class PatientDashboardComponent {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
