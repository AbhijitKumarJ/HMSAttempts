
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-admin-audit',
  standalone: true,
  imports: [JsonPipe],
  template: `
    <div class="space-y-6">
       <div class="flex justify-between items-center">
        <div>
          <h1 class="text-2xl font-bold text-slate-900">Audit Logs</h1>
          <p class="text-slate-500 text-sm">Immutable record of system actions.</p>
        </div>
        <div class="flex gap-2">
           <input type="date" class="border border-slate-300 rounded-md text-sm p-2">
           <button class="bg-white border border-slate-300 text-slate-700 px-4 py-2 rounded-md hover:bg-slate-50 font-medium text-sm">
             Export CSV
           </button>
        </div>
      </div>

      <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
        <table class="min-w-full divide-y divide-slate-200">
          <thead class="bg-slate-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Timestamp</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">User</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Action</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Entity</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Details</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-200 bg-white">
             @for (log of logs(); track log.id) {
               <tr class="hover:bg-slate-50 group">
                 <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500 font-mono">{{ log.timestamp }}</td>
                 <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-slate-900">{{ log.user }}</td>
                 <td class="px-6 py-4 whitespace-nowrap">
                    <span class="px-2 py-0.5 rounded text-xs font-bold uppercase
                       {{ log.action === 'Update' ? 'bg-blue-50 text-blue-700' : (log.action === 'Login' ? 'bg-green-50 text-green-700' : 'bg-slate-100 text-slate-600') }}">
                       {{ log.action }}
                    </span>
                 </td>
                 <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-600">{{ log.entity }}</td>
                 <td class="px-6 py-4 text-sm text-slate-500">
                    <div>{{ log.details }}</div>
                    @if (log.diff) {
                       <div class="mt-2 text-xs bg-slate-50 p-2 rounded border border-slate-200 font-mono">
                          <div class="text-red-500">- {{ log.diff.old }}</div>
                          <div class="text-green-600">+ {{ log.diff.new }}</div>
                       </div>
                    }
                 </td>
               </tr>
             }
          </tbody>
        </table>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminAuditComponent {
  dataService = inject(DataService);
  logs = this.dataService.auditLogs;
}
