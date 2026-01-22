
import { Component, ChangeDetectionStrategy, inject, signal, effect } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DataService, Patient } from '../services/data.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [RouterLink],
  template: `
    @if (patient()) {
      <div class="max-w-5xl mx-auto space-y-6">
        <!-- Breadcrumb -->
        <nav class="flex text-sm text-slate-500 mb-4">
          <a routerLink="/app/patients" class="hover:text-blue-600">Registry</a>
          <span class="mx-2">/</span>
          <span class="text-slate-900 font-medium">{{ patient()?.firstName }} {{ patient()?.lastName }}</span>
        </nav>

        <!-- Header Card -->
        <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 flex justify-between items-start">
           <div class="flex gap-4">
             <div class="h-16 w-16 bg-blue-100 text-blue-600 rounded-lg flex items-center justify-center text-xl font-bold">
               {{ patient()?.firstName?.charAt(0) }}{{ patient()?.lastName?.charAt(0) }}
             </div>
             <div>
               <h1 class="text-2xl font-bold text-slate-900">{{ patient()?.lastName }}, {{ patient()?.firstName }}</h1>
               <div class="flex gap-4 mt-1 text-sm text-slate-600">
                 <span>{{ patient()?.dob }} (38y)</span>
                 <span>{{ patient()?.gender }}</span>
                 <span class="text-slate-300">|</span>
                 <span>MRN: {{ patient()?.mrn }}</span>
               </div>
               <div class="mt-2 flex gap-2">
                 <span class="px-2 py-0.5 rounded text-xs font-semibold bg-slate-100 text-slate-700">Insurance: {{ patient()?.insurance }}</span>
               </div>
             </div>
           </div>
           
           <div class="flex flex-col items-end gap-2">
              <button class="bg-blue-600 text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-blue-700">Check-In</button>
              <button class="bg-white border border-slate-300 text-slate-700 px-4 py-2 rounded-md text-sm font-medium hover:bg-slate-50">Edit Profile</button>
           </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <!-- Left Column: Quick Info -->
          <div class="space-y-6">
             <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
                <h3 class="font-semibold text-slate-800 mb-4">Contact Information</h3>
                <div class="space-y-3 text-sm">
                  <div>
                    <span class="block text-slate-500 text-xs">Phone</span>
                    <span class="text-slate-900">{{ patient()?.phone }}</span>
                  </div>
                   <div>
                    <span class="block text-slate-500 text-xs">Address</span>
                    <span class="text-slate-900">123 Maple Street<br>Springfield, IL 62704</span>
                  </div>
                </div>
             </div>

             <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
                <h3 class="font-semibold text-slate-800 mb-4">Financial</h3>
                <div class="flex justify-between items-center mb-2">
                   <span class="text-slate-500 text-sm">Outstanding Balance</span>
                   <span class="font-bold text-red-600">\${{ patient()?.balance }}</span>
                </div>
                <button class="w-full mt-2 bg-slate-100 text-slate-700 text-xs font-bold py-2 rounded hover:bg-slate-200">Collect Payment</button>
             </div>
          </div>

          <!-- Main Column: Clinical History / Actions -->
          <div class="md:col-span-2 space-y-6">
             <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 min-h-[400px]">
                <div class="border-b border-slate-100 pb-4 mb-4 flex gap-6">
                   <button class="text-blue-600 font-semibold border-b-2 border-blue-600 pb-4 -mb-4.5">Appointments</button>
                   <button class="text-slate-500 hover:text-slate-700 font-medium">Documents</button>
                   <button class="text-slate-500 hover:text-slate-700 font-medium">History</button>
                </div>

                <div class="space-y-4">
                  <!-- Mock Appointment Item -->
                  <div class="flex items-start p-4 bg-slate-50 rounded-lg border border-slate-200">
                     <div class="flex-shrink-0 h-10 w-10 bg-white rounded-full flex items-center justify-center border border-slate-200 text-slate-400">
                       <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                     </div>
                     <div class="ml-4 flex-1">
                        <div class="flex justify-between">
                          <h4 class="text-sm font-bold text-slate-900">General Check-up</h4>
                          <span class="text-xs text-slate-500">Nov 10, 2023</span>
                        </div>
                        <p class="text-sm text-slate-600 mt-1">Dr. Sarah Mitchell</p>
                     </div>
                     <span class="px-2 py-1 bg-green-100 text-green-800 text-xs rounded font-medium">Completed</span>
                  </div>

                  <div class="flex items-center justify-center p-8 text-slate-400 text-sm border-2 border-dashed border-slate-200 rounded-lg">
                     No upcoming appointments.
                     <a routerLink="/app/schedule/book" class="ml-1 text-blue-600 font-semibold hover:underline">Book Now</a>
                  </div>
                </div>
             </div>
          </div>
        </div>
      </div>
    } @else {
      <div class="flex items-center justify-center h-64 text-slate-500">
        Loading Patient Record...
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientDetailComponent {
  private route = inject(ActivatedRoute);
  private dataService = inject(DataService);
  
  patient = signal<Patient | undefined>(undefined);

  constructor() {
    this.route.paramMap.subscribe(params => {
      const mrn = params.get('mrn');
      if (mrn) {
        // Simulating fetch
        const p = this.dataService.getPatientByMrn(mrn);
        this.patient.set(p);
      }
    });
  }
}
