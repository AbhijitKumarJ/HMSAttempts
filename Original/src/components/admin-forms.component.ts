
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { DataService, FormSchema } from '../services/data.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-forms',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="h-[calc(100vh-100px)] flex flex-col">
       <div class="flex justify-between items-center mb-4">
        <div>
          <h1 class="text-2xl font-bold text-slate-900">Form Builder</h1>
          <p class="text-slate-500 text-sm">Configure clinical intake forms via JSON Schema.</p>
        </div>
        <div class="flex gap-3">
           <select class="border border-slate-300 rounded-md text-sm p-2" (change)="selectForm($any($event.target).value)">
              @for (form of forms(); track form.id) {
                <option [value]="form.id">{{ form.name }} (v{{ form.version }})</option>
              }
           </select>
           <button (click)="save()" class="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 font-medium text-sm shadow-sm">
             Publish Version
           </button>
        </div>
      </div>

      <div class="flex-1 flex gap-6 min-h-0">
         <!-- Left: Editor -->
         <div class="flex-1 flex flex-col">
            <div class="bg-slate-800 text-slate-400 px-4 py-2 text-xs font-mono rounded-t-lg flex justify-between">
               <span>JSON Schema Definition</span>
               <span>READ/WRITE</span>
            </div>
            <textarea [(ngModel)]="currentSchema" class="flex-1 bg-slate-900 text-green-400 font-mono text-sm p-4 rounded-b-lg resize-none focus:outline-none focus:ring-2 focus:ring-indigo-500"></textarea>
         </div>

         <!-- Right: Preview -->
         <div class="flex-1 flex flex-col">
            <div class="bg-white border border-slate-200 px-4 py-2 text-xs font-bold text-slate-500 rounded-t-lg uppercase tracking-wide">
               Live Preview
            </div>
            <div class="flex-1 bg-white border-x border-b border-slate-200 rounded-b-lg p-8 overflow-y-auto">
               <form class="space-y-6 max-w-lg mx-auto">
                  @for (field of parsedSchema(); track field.key) {
                     <div>
                        <label class="block text-sm font-medium text-slate-700 mb-1">
                           {{ field.label }} 
                           @if(field.required) { <span class="text-red-500">*</span> }
                        </label>
                        
                        @if (field.type === 'text') {
                           <input type="text" class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
                        } @else if (field.type === 'number') {
                           <input type="number" class="block w-full border border-slate-300 rounded-md shadow-sm py-2 px-3 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
                        } @else if (field.type === 'boolean') {
                           <div class="flex items-center gap-4 mt-2">
                              <label class="flex items-center gap-2">
                                 <input type="radio" [name]="field.key" class="text-indigo-600 focus:ring-indigo-500"> Yes
                              </label>
                              <label class="flex items-center gap-2">
                                 <input type="radio" [name]="field.key" class="text-indigo-600 focus:ring-indigo-500"> No
                              </label>
                           </div>
                        }
                     </div>
                  } @empty {
                     <div class="text-center text-slate-400 py-10">Invalid JSON or Empty Schema</div>
                  }
               </form>
            </div>
         </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminFormsComponent {
  dataService = inject(DataService);
  forms = this.dataService.formSchemas;
  
  selectedFormId = signal<string>('frm-1');
  currentSchema = signal<string>('');
  
  parsedSchema = signal<any[]>([]);

  constructor() {
    // Init
    this.selectForm('frm-1');
  }

  selectForm(id: string) {
    this.selectedFormId.set(id);
    const form = this.forms().find(f => f.id === id);
    if (form) {
      this.currentSchema.set(form.schema);
      this.updatePreview();
    }
  }

  save() {
    this.dataService.updateFormSchema(this.selectedFormId(), this.currentSchema());
    alert('Form schema published successfully!');
  }

  // Basic reactivity for preview
  updatePreview() {
    try {
       this.parsedSchema.set(JSON.parse(this.currentSchema()));
    } catch (e) {
       this.parsedSchema.set([]);
    }
  }

  // Update preview when textarea changes (using template binding to call this would be better but simple getter works with signal)
  ngDoCheck() {
     // Simple dirty check for demo purposes to keep preview live
     try {
       const parsed = JSON.parse(this.currentSchema());
       // Only update if different to avoid loops if we were deep checking, but simpler here
       // In real app, listen to valueChanges
       if (JSON.stringify(parsed) !== JSON.stringify(this.parsedSchema())) {
          this.parsedSchema.set(parsed);
       }
     } catch(e) {}
  }
}
