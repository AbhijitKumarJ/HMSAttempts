
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { DataService, MedicationTask } from '../services/data.service';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-mar',
  standalone: true,
  imports: [RouterLink],
  template: `
    @if (selectedTask()) {
      <!-- WIZARD MODE (Administering specific med) -->
      <div class="max-w-2xl mx-auto mt-10 space-y-6">
         <!-- Safety Banner -->
         <div class="bg-blue-600 text-white p-4 rounded-t-xl shadow-lg flex justify-between items-center">
            <h2 class="text-lg font-bold flex items-center gap-2">
               <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
               Medication Verification
            </h2>
            <span class="text-blue-100 text-sm font-mono">{{ selectedTask()?.id }}</span>
         </div>
         
         <div class="bg-white rounded-b-xl shadow-sm border border-slate-200 p-8 space-y-8">
            <div class="text-center">
               <p class="text-slate-500 text-sm uppercase tracking-wide">You are administering</p>
               <h1 class="text-4xl font-bold text-slate-900 mt-2">{{ selectedTask()?.drugName }}</h1>
               <p class="text-xl text-emerald-600 font-medium mt-1">{{ selectedTask()?.dose }}</p>
            </div>

            <div class="bg-slate-50 p-6 rounded-lg border border-slate-200">
               <h3 class="text-sm font-bold text-slate-700 uppercase mb-4">5 Rights Safety Check</h3>
               <div class="space-y-3">
                  <label class="flex items-center gap-3 cursor-pointer">
                     <input type="checkbox" class="w-5 h-5 text-emerald-600 rounded focus:ring-emerald-500">
                     <span class="text-slate-700">Right Patient: <strong>{{ selectedTask()?.patientName }}</strong> ({{ selectedTask()?.patientMrn }})</span>
                  </label>
                  <label class="flex items-center gap-3 cursor-pointer">
                     <input type="checkbox" class="w-5 h-5 text-emerald-600 rounded focus:ring-emerald-500">
                     <span class="text-slate-700">Right Drug: <strong>{{ selectedTask()?.drugName }}</strong></span>
                  </label>
                  <label class="flex items-center gap-3 cursor-pointer">
                     <input type="checkbox" class="w-5 h-5 text-emerald-600 rounded focus:ring-emerald-500">
                     <span class="text-slate-700">Right Dose: <strong>{{ selectedTask()?.dose }}</strong></span>
                  </label>
                  <label class="flex items-center gap-3 cursor-pointer">
                     <input type="checkbox" class="w-5 h-5 text-emerald-600 rounded focus:ring-emerald-500">
                     <span class="text-slate-700">Right Route & Time</span>
                  </label>
               </div>
            </div>

            <div class="flex justify-between items-center pt-4">
               <a routerLink="/app/mar" class="text-slate-500 hover:text-slate-700 font-medium">Cancel</a>
               <button class="bg-emerald-600 hover:bg-emerald-700 text-white px-8 py-3 rounded-lg font-bold shadow-lg transform transition-transform active:scale-95">
                  Confirm Dispense
               </button>
            </div>
         </div>
      </div>
    } @else {
      <!-- LIST MODE (Global MAR) -->
      <div class="space-y-6">
        <h1 class="text-2xl font-bold text-slate-900">Medication Administration Record (MAR)</h1>
        <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
           <table class="min-w-full divide-y divide-slate-200">
             <thead class="bg-slate-50">
               <tr>
                 <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase">Patient</th>
                 <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase">Drug</th>
                 <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase">Dose</th>
                 <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase">Due</th>
                 <th class="px-6 py-3 text-right text-xs font-medium text-slate-500 uppercase">Action</th>
               </tr>
             </thead>
             <tbody class="divide-y divide-slate-200">
               @for (task of meds(); track task.id) {
                 <tr class="hover:bg-slate-50">
                   <td class="px-6 py-4">
                      <div class="font-bold text-slate-900">{{ task.patientName }}</div>
                      <div class="text-xs text-slate-500">{{ task.patientMrn }}</div>
                   </td>
                   <td class="px-6 py-4 text-sm text-slate-900">{{ task.drugName }}</td>
                   <td class="px-6 py-4 text-sm text-slate-600">{{ task.dose }}</td>
                   <td class="px-6 py-4">
                      <span class="px-2 py-1 rounded text-xs font-bold 
                        {{ task.status === 'Overdue' ? 'bg-red-100 text-red-800' : 'bg-emerald-100 text-emerald-800' }}">
                        {{ task.dueTime }}
                      </span>
                   </td>
                   <td class="px-6 py-4 text-right">
                      <a [routerLink]="['/app/mar', task.patientMrn, task.id]" class="text-emerald-600 hover:text-emerald-900 font-medium">Administer</a>
                   </td>
                 </tr>
               }
             </tbody>
           </table>
        </div>
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MarComponent {
  dataService = inject(DataService);
  route = inject(ActivatedRoute);
  
  meds = this.dataService.meds;
  selectedTask = signal<MedicationTask | undefined>(undefined);

  constructor() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('orderId');
      if (id) {
        this.selectedTask.set(this.meds().find(m => m.id === id));
      } else {
        this.selectedTask.set(undefined);
      }
    });
  }
}
