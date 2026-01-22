
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { DataService } from '../services/data.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-nurse-patient-list',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <div>
          <h1 class="text-2xl font-bold text-slate-900">My Patient List</h1>
          <p class="text-slate-500 text-sm">Active patients assigned to your unit.</p>
        </div>
      </div>

      <div class="bg-white rounded-lg border border-slate-200 shadow-sm overflow-hidden">
        <table class="min-w-full divide-y divide-slate-200">
          <thead class="bg-slate-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Patient</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Loc</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Acuity</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-wider">Complaint</th>
              <th class="px-6 py-3 text-right text-xs font-medium text-slate-500 uppercase tracking-wider">Actions</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-slate-200">
            @for (patient of patients(); track patient.mrn) {
              <tr class="hover:bg-slate-50">
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm font-bold text-slate-900">{{ patient.lastName }}, {{ patient.firstName }}</div>
                  <div class="text-xs text-slate-500">{{ patient.mrn }}</div>
                   @if (patient.allergies?.length) {
                     <div class="mt-1 inline-flex items-center px-1.5 py-0.5 rounded text-xs font-medium bg-red-100 text-red-800">
                       Allergy: {{ patient.allergies?.[0] }}
                     </div>
                   }
                </td>
                <td class="px-6 py-4 text-sm text-slate-500">ER-{{ patient.mrn.slice(-1) }}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                   <span class="px-2 py-1 rounded-full text-xs font-bold 
                     {{ patient.acuity === 'Critical' ? 'bg-red-100 text-red-800' : (patient.acuity === 'High' ? 'bg-amber-100 text-amber-800' : 'bg-slate-100 text-slate-600') }}">
                     {{ patient.acuity || 'Pending' }}
                   </span>
                </td>
                <td class="px-6 py-4 text-sm text-slate-500">
                   {{ patient.status === 'Pending Triage' ? 'Not Assessed' : 'General Discomfort' }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-2">
                  <a [routerLink]="['/app/triage', patient.mrn]" class="text-emerald-600 hover:text-emerald-900">Triage</a>
                  <span class="text-slate-300">|</span>
                  <a [routerLink]="['/app/mar']" class="text-blue-600 hover:text-blue-900">Meds</a>
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
export class NursePatientListComponent {
  dataService = inject(DataService);
  patients = this.dataService.patients;
}
