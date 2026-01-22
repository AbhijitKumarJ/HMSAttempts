
import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DataService, Patient, TimelineEvent } from '../services/data.service';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-clinical-cockpit',
  standalone: true,
  imports: [RouterLink, FormsModule, DatePipe],
  template: `
    @if (patient()) {
       <div class="flex h-[calc(100vh-64px)] bg-slate-100 overflow-hidden">
          
          <!-- Column 1: Context Rail (20%) -->
          <aside class="w-72 bg-white border-r border-slate-200 flex flex-col flex-shrink-0 z-20 shadow-sm overflow-y-auto">
             <!-- Patient Banner -->
             <div class="p-4 border-b border-slate-100 bg-slate-50">
                <div class="flex items-center gap-3">
                   <div class="h-12 w-12 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center font-bold text-lg">
                      {{ patient()?.firstName?.charAt(0) }}{{ patient()?.lastName?.charAt(0) }}
                   </div>
                   <div>
                      <h2 class="font-bold text-slate-900 leading-tight">{{ patient()?.lastName }}, {{ patient()?.firstName }}</h2>
                      <p class="text-xs text-slate-500 font-mono mt-0.5">{{ patient()?.mrn }}</p>
                   </div>
                </div>
                <div class="mt-3 flex justify-between text-xs text-slate-600 font-medium">
                   <span>{{ patient()?.dob }} (32y)</span>
                   <span>{{ patient()?.gender }}</span>
                </div>
             </div>

             <!-- Allergies -->
             @if (patient()?.allergies?.length) {
                <div class="p-3 bg-red-50 border-b border-red-100">
                   <h3 class="text-xs font-bold text-red-800 uppercase tracking-wider mb-1">Allergies</h3>
                   <div class="flex flex-wrap gap-1">
                      @for (alg of patient()?.allergies; track alg) {
                         <span class="text-xs text-red-700 bg-red-100 border border-red-200 px-1.5 rounded">{{ alg }}</span>
                      }
                   </div>
                </div>
             }

             <!-- Problems -->
             <div class="p-4 border-b border-slate-100">
                <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Active Problems</h3>
                <ul class="space-y-1">
                   @for (prob of patient()?.activeProblems; track prob) {
                      <li class="text-sm text-slate-700">• {{ prob }}</li>
                   } @empty {
                      <li class="text-xs text-slate-400 italic">No active problems</li>
                   }
                </ul>
             </div>

             <!-- Meds -->
             <div class="p-4 border-b border-slate-100">
                <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Current Meds</h3>
                <ul class="space-y-1">
                   @for (med of patient()?.currentMeds; track med) {
                      <li class="text-sm text-slate-700">• {{ med }}</li>
                   } @empty {
                      <li class="text-xs text-slate-400 italic">No active meds</li>
                   }
                </ul>
             </div>

             <!-- Vitals Hx -->
             <div class="p-4">
                <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Last Vitals</h3>
                <div class="bg-slate-50 p-2 rounded text-sm font-mono text-slate-700 border border-slate-200">
                   {{ patient()?.vitalsSummary || 'Not recorded' }}
                </div>
             </div>
          </aside>

          <!-- Column 2: Timeline & Review (45%) -->
          <main class="flex-1 flex flex-col min-w-0 bg-slate-50/50">
             <!-- Tabs -->
             <div class="h-12 bg-white border-b border-slate-200 flex items-center px-4 space-x-6">
                <button class="text-sm font-bold text-blue-600 border-b-2 border-blue-600 h-full">Timeline</button>
                <button class="text-sm font-medium text-slate-500 hover:text-slate-800 h-full">Flowsheets</button>
                <button class="text-sm font-medium text-slate-500 hover:text-slate-800 h-full">Images (0)</button>
             </div>

             <!-- Feed -->
             <div class="flex-1 overflow-y-auto p-6 space-y-6">
                @for (event of patient()?.timeline; track event.id) {
                   <div class="flex gap-4">
                      <!-- Icon -->
                      <div class="flex-shrink-0 mt-1">
                         <div class="h-8 w-8 rounded-full flex items-center justify-center border 
                            {{ event.type === 'Note' ? 'bg-blue-50 border-blue-200 text-blue-600' : 
                               (event.type === 'Order' ? 'bg-purple-50 border-purple-200 text-purple-600' : 
                               (event.type === 'Lab' ? 'bg-emerald-50 border-emerald-200 text-emerald-600' : 'bg-slate-100 border-slate-300 text-slate-500')) }}">
                            @if (event.type === 'Note') { <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg> }
                            @else if (event.type === 'Lab') { <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.384-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" /></svg> }
                            @else { <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" /></svg> }
                         </div>
                      </div>
                      <!-- Content -->
                      <div class="flex-1 bg-white p-4 rounded-lg border border-slate-200 shadow-sm hover:shadow-md transition-shadow">
                         <div class="flex justify-between items-start mb-2">
                            <div>
                               <h4 class="text-sm font-bold text-slate-900">{{ event.title }}</h4>
                               <p class="text-xs text-slate-500">{{ event.author }} • {{ event.timestamp }}</p>
                            </div>
                            @if (event.status) {
                               <span class="px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wide
                                  {{ event.status === 'Completed' || event.status === 'Final' ? 'bg-green-100 text-green-800' : 'bg-slate-100 text-slate-600' }}">
                                  {{ event.status }}
                               </span>
                            }
                         </div>
                         <p class="text-sm text-slate-700 whitespace-pre-wrap">{{ event.details }}</p>
                      </div>
                   </div>
                }
             </div>
          </main>

          <!-- Column 3: Action Panel (35%) -->
          <aside class="w-96 bg-white border-l border-slate-200 flex flex-col flex-shrink-0 shadow-xl z-30">
             <!-- Action Tabs -->
             <div class="flex border-b border-slate-200 bg-slate-50">
                <button (click)="activeTab.set('notes')" class="flex-1 py-3 text-sm font-bold {{ activeTab() === 'notes' ? 'text-blue-700 bg-white border-t-2 border-t-blue-600' : 'text-slate-500 hover:text-slate-700' }}">Notes</button>
                <button (click)="activeTab.set('orders')" class="flex-1 py-3 text-sm font-bold {{ activeTab() === 'orders' ? 'text-blue-700 bg-white border-t-2 border-t-blue-600' : 'text-slate-500 hover:text-slate-700' }}">Orders</button>
                <button (click)="activeTab.set('plan')" class="flex-1 py-3 text-sm font-bold {{ activeTab() === 'plan' ? 'text-blue-700 bg-white border-t-2 border-t-blue-600' : 'text-slate-500 hover:text-slate-700' }}">Diagnosis</button>
             </div>

             <!-- Tab Content -->
             <div class="flex-1 overflow-y-auto p-4">
                
                @if (activeTab() === 'notes') {
                   <div class="h-full flex flex-col">
                      <div class="flex justify-between items-center mb-2">
                         <h3 class="text-sm font-bold text-slate-700">Clinical Note</h3>
                         <span class="text-xs text-slate-400 italic">Auto-saving...</span>
                      </div>
                      <textarea [(ngModel)]="noteContent" class="flex-1 w-full border border-slate-300 rounded-md p-3 text-sm focus:ring-blue-500 focus:border-blue-500 resize-none font-mono leading-relaxed" placeholder="Type .macro for quick text..."></textarea>
                      <div class="mt-4 flex gap-2">
                         <button (click)="insertMacro('.normal')" class="text-xs bg-slate-100 px-2 py-1 rounded border border-slate-200 hover:bg-slate-200">.normal_exam</button>
                         <button (click)="insertMacro('.ros')" class="text-xs bg-slate-100 px-2 py-1 rounded border border-slate-200 hover:bg-slate-200">.ros_neg</button>
                      </div>
                   </div>
                }

                @if (activeTab() === 'orders') {
                   <div class="h-full flex flex-col">
                      <div class="mb-4">
                         <label class="block text-xs font-bold text-slate-500 uppercase mb-1">Search Order Catalog</label>
                         <div class="relative">
                            <input type="text" [(ngModel)]="orderSearch" (keyup.enter)="addOrder()" placeholder="e.g. CBC, Tylenol..." class="w-full border border-slate-300 rounded-md pl-3 pr-10 py-2 text-sm focus:ring-blue-500 focus:border-blue-500">
                            <button (click)="addOrder()" class="absolute right-2 top-2 text-blue-600 hover:text-blue-800">
                               <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
                            </button>
                         </div>
                      </div>

                      <div class="flex-1 overflow-y-auto space-y-2 mb-4">
                         @for (ord of pendingOrders(); track ord) {
                            <div class="flex justify-between items-center bg-blue-50 border border-blue-100 p-3 rounded-md">
                               <span class="text-sm font-medium text-blue-900">{{ ord }}</span>
                               <button (click)="removeOrder(ord)" class="text-blue-400 hover:text-red-500">
                                  <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
                               </button>
                            </div>
                         }
                         @if (pendingOrders().length === 0) {
                            <div class="text-center py-10 text-slate-400 text-sm border-2 border-dashed border-slate-100 rounded-lg">
                               Basket is empty.
                            </div>
                         }
                      </div>
                      
                      <button (click)="signOrders()" [disabled]="pendingOrders().length === 0" class="w-full py-3 bg-blue-600 text-white font-bold rounded-lg shadow hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed">
                         Sign & Submit ({{ pendingOrders().length }})
                      </button>
                   </div>
                }

                @if (activeTab() === 'plan') {
                    <div class="space-y-4">
                       <div>
                          <label class="block text-xs font-bold text-slate-500 uppercase mb-1">Primary Diagnosis (ICD-10)</label>
                          <input type="text" placeholder="Search diagnosis..." class="w-full border border-slate-300 rounded-md p-2 text-sm">
                       </div>
                       <div>
                          <label class="block text-xs font-bold text-slate-500 uppercase mb-1">Disposition</label>
                          <select class="w-full border border-slate-300 rounded-md p-2 text-sm bg-white">
                             <option>Discharge Home</option>
                             <option>Admit to Inpatient</option>
                             <option>Transfer</option>
                          </select>
                       </div>
                    </div>
                }
             </div>
          </aside>
       </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClinicalCockpitComponent {
  private route = inject(ActivatedRoute);
  private dataService = inject(DataService);

  patient = signal<Patient | undefined>(undefined);
  activeTab = signal<'notes' | 'orders' | 'plan'>('notes');
  
  // Note State
  noteContent = '';
  
  // Order State
  orderSearch = '';
  pendingOrders = signal<string[]>([]);

  constructor() {
    this.route.paramMap.subscribe(params => {
      const mrn = params.get('mrn');
      if (mrn) {
        this.patient.set(this.dataService.getPatientByMrn(mrn));
      }
    });
  }

  insertMacro(macro: string) {
    if (macro === '.normal') this.noteContent += ' Patient is well-developed, well-nourished, in no acute distress. Lungs clear. Heart RRR.';
    if (macro === '.ros') this.noteContent += ' ROS: Negative for fever, chills, chest pain, shortness of breath.';
  }

  addOrder() {
    if (this.orderSearch.trim()) {
      this.pendingOrders.update(o => [...o, this.orderSearch.trim()]);
      this.orderSearch = '';
    }
  }

  removeOrder(ord: string) {
    this.pendingOrders.update(o => o.filter(i => i !== ord));
  }

  signOrders() {
    const orders = this.pendingOrders();
    const p = this.patient();
    if (p && orders.length > 0) {
      // Create Event
      const newEvent: TimelineEvent = {
        id: `ev-${Date.now()}`,
        type: 'Order',
        title: 'Physician Orders',
        timestamp: 'Just now',
        author: 'Dr. Sarah',
        details: orders.join(', '),
        status: 'Pending'
      };
      
      this.dataService.addTimelineEvent(p.mrn, newEvent);
      this.pendingOrders.set([]);
      
      // Force refresh of local signal since patient object is mutable but signal check might be shallow
      this.patient.set({ ...p });
    }
  }
}
