import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-receptionist-dashboard',
  standalone: true,
  templateUrl: './receptionist-dashboard.component.html',
  styleUrls: ['./receptionist-dashboard.component.scss']
})
export class ReceptionistDashboardComponent {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
