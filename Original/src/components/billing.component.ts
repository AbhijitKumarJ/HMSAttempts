
import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-billing',
  standalone: true,
  template: `
    <div class="flex flex-col items-center justify-center h-full text-center space-y-4">
      <div class="bg-blue-50 p-6 rounded-full">
        <svg class="h-12 w-12 text-blue-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
      </div>
      <h2 class="text-xl font-bold text-slate-900">Billing Module</h2>
      <p class="text-slate-500 max-w-sm">This module lists unpaid invoices and allows payment collection. Currently under maintenance in this demo.</p>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BillingComponent {}
