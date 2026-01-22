
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DataService, Patient } from '../services/data.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-triage',
  standalone: true,
  imports: [RouterLink, FormsModule],
  template: `
    @if (patient()) {
       <!-- Persistent Safety Header -->
       @if (patient()?.allergies?.length) {
         <div class="bg-red-600 text-white px-6 py-2 flex items-center justify-between shadow-md">
            <span class="font-bold uppercase tracking-wider text-sm flex items-center gap-2">
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
              Allergies: {{ patient()?.allergies?.join(', ') }}
            </span>
            <button class="text-xs border border-white/30 px-2 py-1 rounded hover:bg-white/10">Acknowledge</button>
         </div>
       }

      <div class="flex h-[calc(100vh-100px)]">
         <!-- Left Panel: Context -->
         <div class="w-80 bg-white border-r border-slate-200 p-6 space-y-6 overflow-y-auto">
            <div>
               <h2 class="text-2xl font-bold text-slate-900">{{ patient()?.lastName }}, {{ patient()?.firstName }}</h2>
               <p class="text-slate-500">{{ patient()?.mrn }}</p>
               <div class="mt-4 space-y-2 text-sm text-slate-600">
                  <div class="flex justify-between"><span>DOB:</span> <span class="font-medium text-slate-900">{{ patient()?.dob }}</span></div>
                  <div class="flex justify-between"><span>Gender:</span> <span class="font-medium text-slate-900">{{ patient()?.gender }}</span></div>
               </div>
            </div>
            
            <div class="border-t border-slate-100 pt-4">
               <h3 class="font-semibold text-slate-800 mb-2">History</h3>
               <p class="text-xs text-slate-500">Hypertension, T2DM. Last visit 3 months ago for med refill.</p>
            </div>
         </div>

         <!-- Right Panel: Form Engine -->
         <div class="flex-1 bg-slate-50 p-8 overflow-y-auto">
            <div class="max-w-3xl mx-auto space-y-8">
               <div class="flex justify-between items-center">
                  <h1 class="text-2xl font-bold text-slate-800">Triage Assessment</h1>
                  <span class="bg-amber-100 text-amber-800 text-xs font-bold px-2 py-1 rounded">Status: In Progress</span>
               </div>

               <!-- Vitals Block -->
               <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
                  <h3 class="text-lg font-semibold text-slate-800 mb-4 flex items-center gap-2">
                     <svg class="w-5 h-5 text-emerald-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/></svg>
                     Vital Signs
                  </h3>
                  <div class="grid grid-cols-2 md:grid-cols-4 gap-6">
                     <div>
                        <label class="block text-xs font-bold text-slate-500 uppercase">Temp (°C)</label>
                        <input type="number" [(ngModel)]="temp" class="mt-1 block w-full border border-slate-300 rounded-md p-2 text-lg font-mono focus:ring-emerald-500 focus:border-emerald-500 {{ temp > 38 ? 'border-red-500 text-red-600 bg-red-50' : '' }}">
                     </div>
                     <div>
                        <label class="block text-xs font-bold text-slate-500 uppercase">HR (bpm)</label>
                        <input type="number" class="mt-1 block w-full border border-slate-300 rounded-md p-2 text-lg font-mono focus:ring-emerald-500 focus:border-emerald-500">
                     </div>
                      <div>
                        <label class="block text-xs font-bold text-slate-500 uppercase">BP (mmHg)</label>
                        <input type="text" placeholder="120/80" class="mt-1 block w-full border border-slate-300 rounded-md p-2 text-lg font-mono focus:ring-emerald-500 focus:border-emerald-500">
                     </div>
                      <div>
                        <label class="block text-xs font-bold text-slate-500 uppercase">SpO2 (%)</label>
                        <input type="number" class="mt-1 block w-full border border-slate-300 rounded-md p-2 text-lg font-mono focus:ring-emerald-500 focus:border-emerald-500">
                     </div>
                  </div>
               </div>

               <!-- Chief Complaint -->
               <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
                  <h3 class="text-lg font-semibold text-slate-800 mb-4">Chief Complaint</h3>
                  <div>
                     <input type="text" [(ngModel)]="complaint" placeholder="Type complaint (e.g., Chest Pain)..." class="w-full border border-slate-300 rounded-md p-3 focus:ring-emerald-500 focus:border-emerald-500">
                  </div>
                  @if (complaint.toLowerCase().includes('chest')) {
                     <div class="mt-4 p-4 bg-blue-50 border border-blue-200 rounded-lg flex items-start gap-3 animate-fade-in">
                        <svg class="w-6 h-6 text-blue-500 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
                        <div>
                           <h4 class="text-sm font-bold text-blue-900">Protocol Suggestion: Cardiac Assessment</h4>
                           <p class="text-xs text-blue-700 mt-1">Order ECG, Troponin I, CXR. Prepare Aspirin.</p>
                           <button class="mt-2 bg-blue-600 text-white text-xs px-3 py-1 rounded hover:bg-blue-700">Apply Protocol</button>
                        </div>
                     </div>
                  }
               </div>

               <!-- Actions -->
               <div class="flex justify-end gap-4 pt-4">
                  <a routerLink="/app/dashboard" class="px-6 py-3 border border-slate-300 rounded-lg text-slate-700 font-medium hover:bg-slate-50">Cancel</a>
                  <button class="px-6 py-3 bg-emerald-600 rounded-lg text-white font-bold hover:bg-emerald-700 shadow-lg flex items-center gap-2">
                     <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"/></svg>
                     Save & Flag for Doctor
                  </button>
               </div>
            </div>
         </div>
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TriageComponent {
  private route = inject(ActivatedRoute);
  private dataService = inject(DataService);
  
  patient = signal<Patient | undefined>(undefined);
  
  // Form State
  temp = 36.5;
  complaint = '';

  constructor() {
    this.route.paramMap.subscribe(params => {
      const mrn = params.get('mrn');
      if (mrn) {
        this.patient.set(this.dataService.getPatientByMrn(mrn));
      }
    });
  }
}
