
import { Component, ChangeDetectionStrategy, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-registration-modal',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="fixed inset-0 z-50 overflow-y-auto" aria-labelledby="modal-title" role="dialog" aria-modal="true">
      <!-- Backdrop -->
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div class="fixed inset-0 bg-slate-900 bg-opacity-75 transition-opacity" (click)="close.emit()"></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <!-- Panel -->
        <div class="inline-block align-bottom bg-white rounded-lg px-4 pt-5 pb-4 text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full sm:p-6">
          
          @if (!generatedMrn()) {
            <!-- Step 1: Form -->
            <div>
              <div class="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-blue-100">
                <svg class="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
                </svg>
              </div>
              <div class="mt-3 text-center sm:mt-5">
                <h3 class="text-lg leading-6 font-medium text-slate-900" id="modal-title">New Patient Registration</h3>
                <p class="text-sm text-slate-500 mt-2">Enter minimal details to generate an MRN and add to queue.</p>
              </div>
              
              <div class="mt-5 space-y-4">
                <div class="grid grid-cols-2 gap-4">
                  <div>
                    <label class="block text-xs font-medium text-slate-700">First Name</label>
                    <input type="text" [(ngModel)]="firstName" class="mt-1 block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                  </div>
                  <div>
                    <label class="block text-xs font-medium text-slate-700">Last Name</label>
                    <input type="text" [(ngModel)]="lastName" class="mt-1 block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                  </div>
                </div>
                
                <div class="grid grid-cols-2 gap-4">
                  <div>
                    <label class="block text-xs font-medium text-slate-700">Date of Birth</label>
                    <input type="date" [(ngModel)]="dob" class="mt-1 block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                  </div>
                   <div>
                    <label class="block text-xs font-medium text-slate-700">Gender</label>
                    <select [(ngModel)]="gender" class="mt-1 block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm bg-white">
                      <option>M</option>
                      <option>F</option>
                      <option>X</option>
                    </select>
                  </div>
                </div>
              </div>
            </div>
            
            <div class="mt-5 sm:mt-6 sm:grid sm:grid-cols-2 sm:gap-3 sm:grid-flow-row-dense">
              <button type="button" (click)="generate()" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none sm:col-start-2 sm:text-sm">
                Generate MRN
              </button>
              <button type="button" (click)="close.emit()" class="mt-3 w-full inline-flex justify-center rounded-md border border-slate-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-slate-700 hover:bg-slate-50 focus:outline-none sm:mt-0 sm:col-start-1 sm:text-sm">
                Cancel
              </button>
            </div>
          } @else {
            <!-- Step 2: Success -->
            <div>
              <div class="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-green-100">
                <svg class="h-6 w-6 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
              </div>
              <div class="mt-3 text-center sm:mt-5">
                <h3 class="text-lg leading-6 font-medium text-slate-900">Registration Successful</h3>
                <div class="mt-4 p-4 bg-slate-50 rounded-lg border border-slate-200">
                  <p class="text-sm text-slate-500 uppercase tracking-wide">Generated MRN</p>
                  <p class="text-2xl font-mono font-bold text-slate-900 tracking-wider">{{ generatedMrn() }}</p>
                </div>
                <p class="text-sm text-slate-500 mt-4">Patient has been added to the triage queue.</p>
              </div>
            </div>
             <div class="mt-5 sm:mt-6">
              <button type="button" (click)="close.emit()" class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none sm:text-sm">
                Done
              </button>
            </div>
          }
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegistrationModalComponent {
  close = output<void>();
  
  firstName = '';
  lastName = '';
  dob = '';
  gender = 'M';
  
  generatedMrn = signal<string | null>(null);

  generate() {
    // Simulate API call
    setTimeout(() => {
      this.generatedMrn.set(`MRN-${Math.floor(1000 + Math.random() * 9000)}`);
    }, 500);
  }
}
