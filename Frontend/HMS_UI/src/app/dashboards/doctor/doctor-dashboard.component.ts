import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  templateUrl: './doctor-dashboard.component.html',
  styleUrls: ['./doctor-dashboard.component.scss']
})
export class DoctorDashboardComponent {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
