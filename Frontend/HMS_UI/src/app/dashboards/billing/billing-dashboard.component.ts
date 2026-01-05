import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
    selector: 'app-billing-dashboard',
    standalone: true,
    template: `
    <div style="padding: 2rem;">
      <h1>Billing Dashboard</h1>
      <p>Welcome, Billing Clerk. Manage invoices and payments.</p>
      <button (click)="logout()">Logout</button>
    </div>
  `
})
export class BillingDashboardComponent {
    private authService = inject(AuthService);

    logout() {
        this.authService.logout();
    }
}
