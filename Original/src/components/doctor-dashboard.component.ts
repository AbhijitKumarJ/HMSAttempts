
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="flex flex-col lg:flex-row gap-6 h-full">
      <!-- Main Queue (75%) -->
      <div class="flex-1 flex flex-col space-y-4">
         <div class="flex justify-between items-center">
            <h1 class="text-2xl font-bold text-slate-800">My Active Queue</h1>
            <div class="flex gap-2">
               <button class="px-3 py-1 text-sm font-medium bg-blue-100 text-blue-800 rounded-md hover:bg-blue-200">Active</button>
               <button class="px-3 py-1 text-sm font-medium bg-white border border-slate-300 text-slate-600 rounded-md hover:bg-slate-50">All</button>
            </div>
         </div>

         <div class="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden flex-1">
            <div class="overflow-x-auto">
               <table class="min-w-full divide-y divide-slate-200">
                  <thead class="bg-slate-50">
                     <tr>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Acuity</th>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Patient</th>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Wait</th>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Complaint</th>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Vitals</th>
                        <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Status</th>
                        <th class="px-6 py-3 text-right text-xs font-bold text-slate-500 uppercase tracking-wider">Action</th>
                     </tr>
                  </thead>
                  <tbody class="bg-white divide-y divide-slate-200">
                     @for (patient of queue(); track patient.mrn) {
                        <tr class="hover:bg-slate-50 group cursor-pointer transition-colors">
                           <td class="px-6 py-4 whitespace-nowrap">
                              <span class="h-3 w-3 rounded-full inline-block 
                                 {{ patient.acuity === 'Critical' ? 'bg-red-500 shadow-sm shadow-red-500/50' : 
                                    (patient.acuity === 'High' ? 'bg-orange-500' : 'bg-green-500') }}">
                              </span>
                           </td>
                           <td class="px-6 py-4">
                              <div class="text-sm font-bold text-slate-900">{{ patient.lastName }}, {{ patient.firstName }}</div>
                              <div class="text-xs text-slate-500">{{ patient.mrn }} • {{ patient.gender }}</div>
                           </td>
                           <td class="px-6 py-4 text-sm text-slate-600">25m</td>
                           <td class="px-6 py-4 text-sm text-slate-800 font-medium">{{ patient.chiefComplaint || 'Unknown' }}</td>
                           <td class="px-6 py-4 text-xs font-mono text-slate-600 bg-slate-50/50 rounded">{{ patient.vitalsSummary || '--' }}</td>
                           <td class="px-6 py-4">
                              <span class="px-2 py-1 rounded text-xs font-bold 
                                 {{ patient.status === 'Ready for Exam' ? 'bg-green-100 text-green-800' : 'bg-slate-100 text-slate-700' }}">
                                 {{ patient.status }}
                              </span>
                           </td>
                           <td class="px-6 py-4 text-right">
                              <a [routerLink]="['/app/clinical/cockpit', patient.mrn]" class="text-white bg-blue-600 hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-bold shadow-sm group-hover:shadow-md transition-all">Open Chart</a>
                           </td>
                        </tr>
                     } @empty {
                        <tr>
                           <td colspan="7" class="px-6 py-12 text-center text-slate-500">
                              No active patients in queue.
                           </td>
                        </tr>
                     }
                  </tbody>
               </table>
            </div>
         </div>
      </div>

      <!-- Results Sidebar (25%) -->
      <div class="w-full lg:w-80 flex flex-col space-y-4">
         <h2 class="text-lg font-bold text-slate-800">Results to Review</h2>
         
         <div class="bg-white rounded-xl shadow-sm border border-slate-200 flex-1 overflow-y-auto p-4 space-y-3">
            @for (item of inbox(); track item.id) {
               <div class="p-3 border rounded-lg {{ item.flag === 'Critical' ? 'border-red-200 bg-red-50' : 'border-slate-200 hover:bg-slate-50' }} transition-colors cursor-pointer">
                  <div class="flex justify-between items-start mb-1">
                     <span class="text-xs font-bold text-slate-500">{{ item.timestamp }}</span>
                     @if (item.flag === 'Critical') {
                        <span class="text-[10px] uppercase font-bold text-red-600 bg-red-100 px-1 rounded">Critical</span>
                     }
                  </div>
                  <div class="font-bold text-slate-900 text-sm">{{ item.patientName }}</div>
                  <div class="text-xs text-slate-500 mb-2">{{ item.testName }}</div>
                  <div class="flex justify-between items-center">
                     <span class="text-lg font-bold {{ item.flag === 'Critical' ? 'text-red-700' : 'text-slate-800' }}">{{ item.value }}</span>
                     <button class="text-xs text-blue-600 font-semibold hover:underline">Ack</button>
                  </div>
               </div>
            } @empty {
               <div class="text-center text-slate-400 py-10 text-sm">
                  All results reviewed.
               </div>
            }
         </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorDashboardComponent {
  dataService = inject(DataService);
  
  queue = this.dataService.getDoctorQueue.bind(this.dataService);
  inbox = this.dataService.resultInbox;
}
