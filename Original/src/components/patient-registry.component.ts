
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-patient-registry',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <div>
          <h1 class="text-2xl font-bold text-slate-900">Patient Registry</h1>
          <p class="text-slate-500 text-sm">Search and manage all registered patients.</p>
        </div>
        <button class="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 font-medium text-sm">
          + Add New Patient
        </button>
      </div>

      <!-- Search Bar -->
      <div class="bg-white p-4 rounded-lg border border-slate-200 shadow-sm flex gap-4">
        <input type="text" placeholder="Search by name, MRN..." class="flex-1 border border-slate-300 rounded-md px-3 py-2 text-sm focus:ring-blue-500 focus:border-blue-500 outline-none">
        <select class="border border-slate-300 rounded-md px-3 py-2 text-sm bg-slate-50 outline-none">
          <option>All Statuses</option>
          <option>Active</option>
          <option>Archived</option>
        </select>
        <button class="px-4 py-2 bg-slate-100 text-slate-700 font-medium rounded-md hover:bg-slate-200 text-sm">Filter</button>
      </div>

      <!-- Patient Grid -->
      <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
        <table class="min-w-full divide-y divide-slate-200">
          <thead class="bg-slate-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Patient</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">MRN</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Phone</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Last Visit</th>
              <th class="px-6 py-3 text-right text-xs font-medium text-slate-500 uppercase tracking-wider">View</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-slate-200">
            @for (patient of patients(); track patient.mrn) {
              <tr class="hover:bg-slate-50">
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm font-medium text-slate-900">{{ patient.lastName }}, {{ patient.firstName }}</div>
                  <div class="text-xs text-slate-500">{{ patient.dob }}</div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{{ patient.mrn }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{{ patient.phone }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{{ patient.lastVisit }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <a [routerLink]="['/app/patients', patient.mrn]" class="text-blue-600 hover:text-blue-900">Open Record</a>
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
export class PatientRegistryComponent {
  dataService = inject(DataService);
  patients = this.dataService.patients;
}
