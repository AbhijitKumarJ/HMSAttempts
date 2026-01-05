import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-nurse-dashboard',
  standalone: true,
  templateUrl: './nurse-dashboard.component.html',
  styleUrls: ['./nurse-dashboard.component.scss']
})
export class NurseDashboardComponent {
  // Rebuild trigger
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
