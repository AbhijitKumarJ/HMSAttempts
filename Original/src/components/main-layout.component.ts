
import { Component, ChangeDetectionStrategy, inject, computed, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="flex h-screen bg-slate-50 overflow-hidden">
      <!-- Fixed Sidebar -->
      <aside class="w-64 flex flex-col flex-shrink-0 transition-all duration-300 {{ themeConfig().sidebar }} text-white">
        <div class="h-16 flex items-center px-6 shadow-sm {{ themeConfig().sidebarHeader }}">
           <div class="w-8 h-8 rounded-lg flex items-center justify-center text-white font-bold text-lg mr-3 {{ themeConfig().logo }}">H</div>
           <span class="text-lg font-bold tracking-wide">HMS Core</span>
        </div>

        <nav class="flex-1 px-4 py-6 space-y-1 overflow-y-auto">
          @if (role() === 'Receptionist') {
            <!-- Receptionist Links -->
            <a routerLink="/app/dashboard" routerLinkActive="bg-blue-600 text-white shadow-lg" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-800 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z" /></svg>
              Dashboard
            </a>
            <a routerLink="/app/patients" routerLinkActive="bg-blue-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-800 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
              Patient Registry
            </a>
            <a routerLink="/app/schedule" routerLinkActive="bg-blue-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-800 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
              Schedule
            </a>
            <a routerLink="/app/billing" routerLinkActive="bg-blue-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-800 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 14l6-6m-5.5.5h.01m4.99 5h.01M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16l3.5-2 3.5 2 3.5-2 3.5 2zM10 8.5a.5.5 0 11-1 0 .5.5 0 011 0zm5 5a.5.5 0 11-1 0 .5.5 0 011 0z" /></svg>
               Billing & Payments
            </a>
          } @else if (role() === 'Nurse') {
            <!-- Nurse Links -->
            <a routerLink="/app/dashboard" routerLinkActive="bg-emerald-600 text-white shadow-lg" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-emerald-100 hover:bg-emerald-800 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
               Triage Station
            </a>
            <a routerLink="/app/nurse-patients" routerLinkActive="bg-emerald-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-emerald-100 hover:bg-emerald-800 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
               My Patient List
            </a>
            <a routerLink="/app/mar" routerLinkActive="bg-emerald-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-emerald-100 hover:bg-emerald-800 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.384-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" /></svg>
               Medication Admin
            </a>
            <a routerLink="/app/labs" routerLinkActive="bg-emerald-600 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-emerald-100 hover:bg-emerald-800 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.384-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" /></svg>
               Lab Requests
            </a>
          } @else if (role() === 'Doctor') {
             <!-- Doctor Links -->
            <a routerLink="/app/doctor-dashboard" routerLinkActive="bg-blue-700 text-white shadow-lg" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
              My Queue
            </a>
            <a routerLink="/app/patients" routerLinkActive="bg-blue-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
              All Patients
            </a>
            <a routerLink="/app/schedule" routerLinkActive="bg-blue-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
              On-Call Schedule
            </a>
             <a routerLink="/app/doctor-dashboard" routerLinkActive="bg-blue-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
               Clinical Reports
            </a>
          } @else if (role() === 'Admin') {
            <!-- Admin Links -->
             <a routerLink="/app/admin/dashboard" routerLinkActive="bg-indigo-700 text-white shadow-lg" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
               System Pulse
            </a>
             <a routerLink="/app/admin/users" routerLinkActive="bg-indigo-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
               User Management
            </a>
             <a routerLink="/app/admin/forms" routerLinkActive="bg-indigo-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" /></svg>
               Form Builder
            </a>
             <a routerLink="/app/admin/audit" routerLinkActive="bg-indigo-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
               Audit Logs
            </a>
             <a routerLink="/app/admin/health" routerLinkActive="bg-indigo-700 text-white shadow-lg" class="group flex items-center px-3 py-2.5 text-sm font-medium rounded-lg text-slate-300 hover:bg-slate-700 hover:text-white transition-all">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h14M5 12a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v4a2 2 0 01-2 2M5 12a2 2 0 00-2 2v4a2 2 0 002 2h14a2 2 0 002-2v-4a2 2 0 00-2-2m-2-4h.01M17 16h.01" /></svg>
               Infrastructure
            </a>
          }
        </nav>

        <div class="p-4 {{ themeConfig().sidebarHeader }}">
          <button (click)="auth.logout()" class="w-full flex items-center px-3 py-2 text-sm font-medium text-slate-400 hover:text-white transition-colors">
            <svg class="mr-3 h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" /></svg>
            Sign Out
          </button>
        </div>
      </aside>

      <!-- Main Content Area -->
      <div class="flex-1 flex flex-col min-w-0 overflow-hidden">
        <!-- Global Header -->
        <header class="h-16 bg-white shadow-sm flex items-center justify-between px-6 z-10 border-b border-slate-200">
          
          <!-- Global Search (Different for Admin) -->
          <div class="flex-1 max-w-2xl flex items-center gap-4">
            <div class="relative flex-1">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
              </div>
              @if(role() === 'Admin') {
                <input type="text" class="block w-full pl-10 pr-3 py-2 border border-slate-300 rounded-md leading-5 bg-slate-50 placeholder-slate-500 focus:outline-none focus:bg-white focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm transition-colors" placeholder="Search Settings, Users, or Logs... (Ctrl+K)">
              } @else {
                <input type="text" class="block w-full pl-10 pr-3 py-2 border border-slate-300 rounded-md leading-5 bg-slate-50 placeholder-slate-500 focus:outline-none focus:bg-white focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors" placeholder="Search Patient by Name, MRN, or Phone... (Ctrl+K)">
              }
            </div>
            
            <!-- Admin System Health Indicator -->
            @if (role() === 'Admin') {
              <div class="flex items-center gap-2 px-3 py-1 bg-green-50 text-green-700 rounded-full text-xs font-bold border border-green-200 cursor-pointer">
                 <span class="w-2 h-2 rounded-full bg-green-500 animate-pulse"></span>
                 System Healthy
              </div>
            }

            <!-- Nurse Alert Ticker -->
            @if (role() === 'Nurse') {
               <div class="hidden lg:flex bg-red-50 text-red-700 px-3 py-1 rounded-full text-xs font-bold items-center gap-2 animate-pulse cursor-pointer">
                  <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                  CRITICAL: Lab Value - Ross, Michael (K+ 6.5)
               </div>
            }
          </div>

          <!-- Right Actions -->
          <div class="ml-4 flex items-center md:ml-6 space-x-4">
            
            <!-- Doctor Results Badge -->
            @if (role() === 'Doctor') {
               <button class="relative p-2 text-slate-400 hover:text-slate-500">
                  <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" /></svg>
                  <span class="absolute top-1 right-1 block h-4 w-4 rounded-full bg-red-500 text-white text-[10px] font-bold flex items-center justify-center">{{ inboxCount() }}</span>
               </button>
            }

            <!-- Role Switcher -->
            <div class="relative">
              <button (click)="toggleRole()" class="flex items-center space-x-2 text-sm font-medium text-slate-700 hover:text-blue-600 focus:outline-none transition-colors">
                <span class="py-1 px-2.5 rounded-full text-xs font-semibold uppercase tracking-wide {{ themeConfig().roleBadge }}">
                  Role: {{ role() }}
                </span>
                <svg class="h-4 w-4 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" /></svg>
              </button>
            </div>

            <!-- Profile -->
            <div class="flex items-center space-x-3 border-l border-slate-200 pl-4">
              <div class="h-9 w-9 rounded-full bg-slate-200 flex items-center justify-center text-slate-600 font-bold border border-slate-300">
                 {{ auth.currentUser().name.substring(0,2).toUpperCase() }}
              </div>
            </div>
          </div>
        </header>

        <!-- Main Scrollable Area -->
        <main class="flex-1 overflow-auto bg-slate-50 p-6">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainLayoutComponent {
  auth = inject(AuthService);
  dataService = inject(DataService);
  role = this.auth.activeRole;

  inboxCount = computed(() => this.dataService.resultInbox().length);

  themeConfig = computed(() => {
    switch (this.role()) {
      case 'Receptionist':
        return { 
          sidebar: 'bg-slate-900', 
          sidebarHeader: 'bg-slate-950', 
          logo: 'bg-blue-600',
          roleBadge: 'bg-blue-100 text-blue-800'
        };
      case 'Nurse':
        return { 
          sidebar: 'bg-emerald-950', 
          sidebarHeader: 'bg-emerald-900', 
          logo: 'bg-emerald-500',
          roleBadge: 'bg-emerald-100 text-emerald-800'
        };
      case 'Doctor':
        return { 
          sidebar: 'bg-slate-800', 
          sidebarHeader: 'bg-slate-900', 
          logo: 'bg-blue-700',
          roleBadge: 'bg-indigo-100 text-indigo-800'
        };
      case 'Admin':
        return { 
          sidebar: 'bg-gray-900', 
          sidebarHeader: 'bg-gray-950', 
          logo: 'bg-indigo-600',
          roleBadge: 'bg-purple-100 text-purple-800'
        };
      default:
        return { sidebar: 'bg-slate-900', sidebarHeader: 'bg-slate-950', logo: 'bg-blue-600', roleBadge: 'bg-blue-100' };
    }
  });

  toggleRole() {
    // Cycle through roles for demo
    const current = this.role();
    let next: any = 'Receptionist';
    if (current === 'Receptionist') next = 'Nurse';
    else if (current === 'Nurse') next = 'Doctor';
    else if (current === 'Doctor') next = 'Admin';
    else next = 'Receptionist';
    
    this.auth.switchRole(next);
  }
}
