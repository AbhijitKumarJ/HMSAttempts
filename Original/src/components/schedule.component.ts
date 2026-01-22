
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-schedule',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
         <h1 class="text-2xl font-bold text-slate-900">Provider Schedule</h1>
         <div class="flex space-x-2">
            <button class="px-3 py-1 border border-slate-300 rounded text-sm text-slate-600 hover:bg-slate-50">Today</button>
            <div class="flex border border-slate-300 rounded overflow-hidden">
               <button class="px-3 py-1 bg-slate-100 text-slate-700 text-sm border-r border-slate-300">Week</button>
               <button class="px-3 py-1 bg-white text-slate-600 text-sm hover:bg-slate-50">Day</button>
            </div>
         </div>
      </div>

      <!-- Calendar Grid Mockup -->
      <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
        <div class="grid grid-cols-6 border-b border-slate-200 bg-slate-50">
           <div class="p-4 text-center text-sm font-semibold text-slate-500 border-r border-slate-200">Time</div>
           <div class="p-4 text-center text-sm font-semibold text-slate-700 border-r border-slate-200">Mon 12</div>
           <div class="p-4 text-center text-sm font-semibold text-slate-700 border-r border-slate-200">Tue 13</div>
           <div class="p-4 text-center text-sm font-semibold text-slate-700 border-r border-slate-200">Wed 14</div>
           <div class="p-4 text-center text-sm font-semibold text-slate-700 border-r border-slate-200">Thu 15</div>
           <div class="p-4 text-center text-sm font-semibold text-slate-700">Fri 16</div>
        </div>
        
        <!-- Time Slots -->
        <div class="divide-y divide-slate-200">
           <div class="grid grid-cols-6 min-h-[100px]">
              <div class="p-2 text-xs text-slate-400 text-right pr-4 border-r border-slate-200">9:00 AM</div>
              <div class="p-1 border-r border-slate-200 relative">
                 <div class="absolute inset-1 bg-blue-100 border border-blue-200 rounded p-1 text-xs text-blue-700 overflow-hidden">
                   <strong>Doe, J</strong><br>Check-up
                 </div>
              </div>
              <div class="p-1 border-r border-slate-200"></div>
              <div class="p-1 border-r border-slate-200 relative">
                  <div class="absolute inset-1 bg-emerald-100 border border-emerald-200 rounded p-1 text-xs text-emerald-700">
                   <strong>Smith, A</strong>
                 </div>
              </div>
              <div class="p-1 border-r border-slate-200"></div>
              <div class="p-1"></div>
           </div>

           <div class="grid grid-cols-6 min-h-[100px]">
              <div class="p-2 text-xs text-slate-400 text-right pr-4 border-r border-slate-200">10:00 AM</div>
              <div class="p-1 border-r border-slate-200"></div>
              <div class="p-1 border-r border-slate-200 relative">
                 <div class="absolute inset-1 bg-blue-100 border border-blue-200 rounded p-1 text-xs text-blue-700">
                   <strong>Ross, M</strong><br>Follow-up
                 </div>
              </div>
              <div class="p-1 border-r border-slate-200"></div>
              <div class="p-1 border-r border-slate-200"></div>
              <div class="p-1"></div>
           </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleComponent {}
