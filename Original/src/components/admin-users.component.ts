
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  template: `
    <div class="space-y-6">
       <div class="flex justify-between items-center">
        <div>
          <h1 class="text-2xl font-bold text-slate-900">User Management (IAM)</h1>
          <p class="text-slate-500 text-sm">Manage access, roles, and security policies.</p>
        </div>
        <button class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 font-medium text-sm shadow-sm">
          + Provision User
        </button>
      </div>

      <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
        <table class="min-w-full divide-y divide-slate-200">
          <thead class="bg-slate-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">User</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Roles</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Status</th>
              <th class="px-6 py-3 text-left text-xs font-bold text-slate-500 uppercase tracking-wider">Last Login</th>
              <th class="px-6 py-3 text-right text-xs font-bold text-slate-500 uppercase tracking-wider">Actions</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-200 bg-white">
             @for (user of users(); track user.id) {
               <tr class="hover:bg-slate-50">
                 <td class="px-6 py-4 whitespace-nowrap">
                   <div class="flex items-center">
                     <div class="h-8 w-8 rounded-full bg-slate-200 flex items-center justify-center font-bold text-xs text-slate-600">
                       {{ user.name.charAt(0) }}
                     </div>
                     <div class="ml-3">
                       <div class="text-sm font-medium text-slate-900">{{ user.name }}</div>
                       <div class="text-xs text-slate-500">{{ user.username }}</div>
                     </div>
                   </div>
                 </td>
                 <td class="px-6 py-4 whitespace-nowrap">
                   <div class="flex gap-1">
                     @for (role of user.roles; track role) {
                        <span class="px-2 py-0.5 rounded text-xs font-medium border
                          {{ role === 'Admin' ? 'bg-purple-50 text-purple-700 border-purple-200' : 'bg-slate-100 text-slate-600 border-slate-200' }}">
                          {{ role }}
                        </span>
                     }
                   </div>
                 </td>
                 <td class="px-6 py-4 whitespace-nowrap">
                   <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full 
                     {{ user.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800' }}">
                     {{ user.status }}
                   </span>
                 </td>
                 <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500">
                   {{ user.lastLogin }}
                 </td>
                 <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                   <button class="text-indigo-600 hover:text-indigo-900 mr-3">Edit</button>
                   <button class="text-slate-400 hover:text-red-600">Reset</button>
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
export class AdminUsersComponent {
  dataService = inject(DataService);
  users = this.dataService.users;
}
