
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <div class="max-w-3xl mx-auto space-y-6">
       <div class="flex items-center gap-2 text-sm text-slate-500">
         <a routerLink="/app/schedule" class="hover:text-blue-600">Schedule</a>
         <span>/</span>
         <span class="text-slate-900">New Appointment</span>
       </div>

       <div class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
          <div class="px-6 py-4 border-b border-slate-100 bg-slate-50">
             <h2 class="text-lg font-bold text-slate-900">Book Appointment Slot</h2>
          </div>
          
          <div class="p-6 space-y-6">
            <!-- Provider Selection -->
            <div>
              <label class="block text-sm font-medium text-slate-700 mb-2">Provider</label>
              <select [(ngModel)]="selectedDoctor" class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 bg-white focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                @for (doc of doctors(); track doc.id) {
                  <option [value]="doc.id">{{ doc.name }} ({{ doc.specialty }})</option>
                }
              </select>
            </div>

            <!-- Date & Time -->
            <div class="grid grid-cols-2 gap-6">
               <div>
                 <label class="block text-sm font-medium text-slate-700 mb-2">Date</label>
                 <input type="date" class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
               </div>
               <div>
                 <label class="block text-sm font-medium text-slate-700 mb-2">Time Slot</label>
                 <select class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 bg-white focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                   <option>09:00 AM</option>
                   <option>09:30 AM</option>
                   <option>10:00 AM</option>
                   <option>10:15 AM (Next Free)</option>
                   <option>10:30 AM</option>
                 </select>
               </div>
            </div>

            <!-- Patient Search -->
            <div>
               <label class="block text-sm font-medium text-slate-700 mb-2">Patient</label>
               <input type="text" placeholder="Start typing name or MRN..." class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
               <p class="mt-1 text-xs text-slate-500">Search existing registry.</p>
            </div>
            
             <!-- Notes -->
            <div>
               <label class="block text-sm font-medium text-slate-700 mb-2">Reason for Visit</label>
               <textarea rows="3" class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"></textarea>
            </div>

            <div class="pt-4 border-t border-slate-100 flex justify-end gap-3">
               <a routerLink="/app/schedule" class="px-4 py-2 border border-slate-300 rounded-md text-sm font-medium text-slate-700 hover:bg-slate-50">Cancel</a>
               <button class="px-4 py-2 bg-blue-600 rounded-md text-sm font-medium text-white hover:bg-blue-700 shadow-sm">Confirm Booking</button>
            </div>
          </div>
       </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookingComponent {
  dataService = inject(DataService);
  route = inject(ActivatedRoute);
  
  doctors = this.dataService.doctors;
  selectedDoctor = signal('');

  constructor() {
    this.route.queryParams.subscribe(params => {
      if (params['doctor']) {
        this.selectedDoctor.set(params['doctor']);
      } else {
        // Default to first
        this.selectedDoctor.set(this.doctors()[0].id);
      }
    });
  }
}
