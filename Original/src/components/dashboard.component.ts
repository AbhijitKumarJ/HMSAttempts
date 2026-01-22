
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { DataService } from '../services/data.service';
import { AuthService } from '../services/auth.service';
import { RouterLink } from '@angular/router';
import { RegistrationModalComponent } from './registration-modal.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, RegistrationModalComponent],
  template: `
    @if (role() === 'Receptionist') {
      <!-- RECEPTIONIST DASHBOARD -->
      <div class="space-y-6">
        <!-- Top Row: Stats & Quick Actions -->
        <div class="grid grid-cols-1 md:grid-cols-4 gap-6">
          <button (click)="openRegistration()" class="bg-blue-600 hover:bg-blue-700 transition-colors rounded-xl p-6 shadow-sm flex flex-col items-center justify-center text-white group">
            <div class="bg-white/20 rounded-full p-3 mb-3 group-hover:scale-110 transition-transform">
              <svg class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" /></svg>
            </div>
            <span class="font-semibold text-lg">New Patient Registration</span>
          </button>

          <a routerLink="/app/billing" class="bg-white hover:border-blue-500 border border-slate-200 transition-colors rounded-xl p-6 shadow-sm flex flex-col items-center justify-center text-slate-700 group">
            <div class="bg-emerald-100 text-emerald-600 rounded-full p-3 mb-3 group-hover:bg-emerald-200 transition-colors">
              <svg class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" /></svg>
            </div>
            <span class="font-medium">Collect Payment</span>
          </a>

          <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200 flex flex-col justify-between">
             <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Patients Waiting</div>
             <div class="text-3xl font-bold text-slate-900 mt-2">{{ waitingCount() }}</div>
             <div class="text-xs text-red-500 mt-2 flex items-center font-medium">
               <span class="w-2 h-2 rounded-full bg-red-500 mr-1"></span> 2 > 15m wait
             </div>
          </div>

           <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200 flex flex-col justify-between">
             <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Doctors Active</div>
             <div class="text-3xl font-bold text-slate-900 mt-2">{{ activeDoctorsCount() }}</div>
             <div class="text-xs text-emerald-600 mt-2 font-medium">100% Coverage</div>
          </div>
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <!-- Waiting Room Queue -->
          <div class="lg:col-span-2 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
            <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
              <h3 class="font-semibold text-slate-800 flex items-center gap-2">
                <svg class="w-5 h-5 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                Waiting Room Queue
              </h3>
              <span class="text-xs font-medium bg-blue-100 text-blue-800 px-2 py-1 rounded-full">{{ waitingRoom().length }} active</span>
            </div>
            
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-slate-200">
                <thead class="bg-slate-50">
                  <tr>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Patient Name</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Status</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Wait Time</th>
                    <th class="px-6 py-3 text-right text-xs font-medium text-slate-500 uppercase tracking-wider">Action</th>
                  </tr>
                </thead>
                <tbody class="bg-white divide-y divide-slate-200">
                  @for (patient of waitingRoom(); track patient.mrn) {
                    <tr class="hover:bg-slate-50 transition-colors cursor-pointer group">
                      <td class="px-6 py-4 whitespace-nowrap">
                        <div class="flex items-center">
                          <div class="flex-shrink-0 h-8 w-8 rounded-full bg-slate-200 flex items-center justify-center text-xs font-bold text-slate-600">
                            {{ patient.firstName.charAt(0) }}{{ patient.lastName.charAt(0) }}
                          </div>
                          <div class="ml-4">
                            <div class="text-sm font-medium text-slate-900 group-hover:text-blue-600">{{ patient.lastName }}, {{ patient.firstName }}</div>
                            <div class="text-xs text-slate-500">{{ patient.mrn }}</div>
                          </div>
                        </div>
                      </td>
                      <td class="px-6 py-4 whitespace-nowrap">
                        <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full 
                          {{ patient.status === 'Checked In' ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800' }}">
                          {{ patient.status }}
                        </span>
                      </td>
                      <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500">12m</td>
                      <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                        <a [routerLink]="['/app/patients', patient.mrn]" class="text-blue-600 hover:text-blue-900 bg-blue-50 hover:bg-blue-100 px-3 py-1 rounded-md transition-colors">Process</a>
                      </td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          </div>

          <!-- Provider Status -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col">
            <div class="px-6 py-4 border-b border-slate-100 bg-slate-50">
               <h3 class="font-semibold text-slate-800">Provider Status</h3>
            </div>
            <div class="p-4 space-y-4 flex-1 overflow-y-auto">
               @for (doc of doctors(); track doc.id) {
                 <div class="border border-slate-200 rounded-lg p-4 flex flex-col gap-3 relative overflow-hidden">
                   <div class="absolute left-0 top-0 bottom-0 w-1 
                     {{ doc.status === 'Available' ? 'bg-emerald-500' : (doc.status === 'Busy' ? 'bg-amber-500' : 'bg-slate-300') }}">
                   </div>
                   <div class="flex justify-between items-start pl-2">
                     <div>
                       <h4 class="font-bold text-slate-900 text-sm">{{ doc.name }}</h4>
                       <p class="text-xs text-slate-500">{{ doc.specialty }}</p>
                     </div>
                     <span class="text-xs font-bold px-2 py-0.5 rounded border 
                       {{ doc.status === 'Available' ? 'border-emerald-200 text-emerald-700 bg-emerald-50' : 'border-amber-200 text-amber-700 bg-amber-50' }}">
                       {{ doc.status }}
                     </span>
                   </div>
                   <div class="flex justify-between items-center text-xs pl-2">
                      <span class="text-slate-500">Next Free: <strong class="text-slate-800">{{ doc.nextFreeSlot }}</strong></span>
                      <a [routerLink]="['/app/schedule/book']" [queryParams]="{doctor: doc.id}" class="text-blue-600 font-semibold hover:underline">Book Slot</a>
                   </div>
                 </div>
               }
            </div>
          </div>
        </div>
      </div>
    } @else {
      <!-- NURSE DASHBOARD (TRIAGE STATION) -->
      <div class="space-y-6">
        <!-- Critical Vitals Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
           @for (alert of alerts(); track alert.patientMrn) {
             <div class="bg-white border-l-4 {{ alert.severity === 'Critical' ? 'border-red-500' : 'border-amber-500' }} rounded-r-xl shadow-sm p-4 flex justify-between items-start animate-pulse">
                <div>
                   <h4 class="font-bold text-slate-900">{{ alert.patientName }}</h4>
                   <p class="text-xs text-slate-500">{{ alert.room }} • {{ alert.patientMrn }}</p>
                   <div class="mt-2 text-lg font-bold {{ alert.severity === 'Critical' ? 'text-red-600' : 'text-amber-600' }}">{{ alert.alert }}</div>
                </div>
                <button class="bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-bold px-3 py-1 rounded">Re-Check</button>
             </div>
           }
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          
          <!-- Triage Queue (Priority) -->
          <div class="lg:col-span-2 bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
             <div class="px-6 py-4 border-b border-slate-100 flex justify-between items-center bg-emerald-50">
               <h3 class="font-semibold text-emerald-900 flex items-center gap-2">
                 <svg class="w-5 h-5 text-emerald-600" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
                 Triage Queue
               </h3>
               <span class="text-xs font-medium bg-white text-emerald-800 border border-emerald-200 px-2 py-1 rounded-full">{{ triageQueue().length }} pending</span>
             </div>
             
             <table class="min-w-full divide-y divide-slate-200">
                <thead class="bg-slate-50">
                  <tr>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Patient</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Acuity</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Status</th>
                    <th class="px-6 py-3 text-right text-xs font-medium text-slate-500 uppercase tracking-wider">Action</th>
                  </tr>
                </thead>
                <tbody class="bg-white divide-y divide-slate-200">
                  @for (patient of triageQueue(); track patient.mrn) {
                    <tr class="hover:bg-slate-50 {{ patient.acuity === 'Critical' ? 'bg-red-50' : '' }}">
                      <td class="px-6 py-4">
                         <div class="font-bold text-slate-900">{{ patient.lastName }}, {{ patient.firstName }}</div>
                         <div class="text-xs text-slate-500">{{ patient.mrn }}</div>
                      </td>
                      <td class="px-6 py-4">
                         <span class="px-2 py-1 rounded-full text-xs font-bold 
                           {{ patient.acuity === 'Critical' ? 'bg-red-100 text-red-800' : (patient.acuity === 'High' ? 'bg-amber-100 text-amber-800' : 'bg-slate-100 text-slate-600') }}">
                           {{ patient.acuity || 'Unassessed' }}
                         </span>
                      </td>
                      <td class="px-6 py-4 text-sm text-slate-500">{{ patient.status }}</td>
                      <td class="px-6 py-4 text-right">
                         <a [routerLink]="['/app/triage', patient.mrn]" class="bg-emerald-600 hover:bg-emerald-700 text-white px-3 py-1 rounded text-sm font-medium shadow-sm">Start Triage</a>
                      </td>
                    </tr>
                  }
                </tbody>
             </table>
          </div>

          <!-- Meds Due Task List -->
          <div class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex flex-col">
             <div class="px-6 py-4 border-b border-slate-100 bg-slate-50 flex justify-between items-center">
               <h3 class="font-semibold text-slate-800">Meds Due <span class="text-xs text-slate-400 font-normal ml-1">(Next 1h)</span></h3>
             </div>
             <div class="divide-y divide-slate-100 overflow-y-auto max-h-[400px]">
                @for (task of meds(); track task.id) {
                  <div class="p-4 hover:bg-slate-50 flex justify-between items-center">
                     <div>
                        <div class="flex items-center gap-2">
                           <span class="font-bold text-slate-900 text-sm">{{ task.patientName }}</span>
                           <span class="text-xs text-slate-400">{{ task.patientMrn }}</span>
                        </div>
                        <div class="text-sm text-slate-800 mt-1"><span class="font-semibold text-blue-600">{{ task.drugName }}</span> {{ task.dose }}</div>
                        <div class="text-xs {{ task.status === 'Overdue' ? 'text-red-500 font-bold' : 'text-slate-500' }}">Due: {{ task.dueTime }} ({{ task.status }})</div>
                     </div>
                     <a [routerLink]="['/app/mar', task.patientMrn, task.id]" class="text-emerald-600 hover:bg-emerald-50 border border-emerald-200 hover:border-emerald-300 px-3 py-1 rounded text-xs font-bold transition-colors">
                       Administer
                     </a>
                  </div>
                }
             </div>
          </div>

        </div>
      </div>
    }
    
    <!-- Shared Registration Modal (Accessible to Receptionist, maybe blocked for Nurse but prompt says reuse) -->
    @if (showRegistrationModal()) {
      <app-registration-modal (close)="closeRegistration()"></app-registration-modal>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {
  dataService = inject(DataService);
  auth = inject(AuthService);
  
  role = this.auth.activeRole;

  // Receptionist Data
  waitingRoom = this.dataService.getWaitingRoom.bind(this.dataService);
  doctors = this.dataService.doctors;
  waitingCount = () => this.dataService.patients().filter(p => p.status === 'Checked In' || p.status === 'Waiting').length;
  activeDoctorsCount = () => this.dataService.doctors().filter(d => d.status !== 'Away').length;
  
  // Nurse Data
  triageQueue = this.dataService.getTriageQueue.bind(this.dataService);
  meds = this.dataService.meds;
  alerts = this.dataService.alerts;

  showRegistrationModal = signal(false);

  openRegistration() {
    this.showRegistrationModal.set(true);
  }

  closeRegistration() {
    this.showRegistrationModal.set(false);
  }
}
