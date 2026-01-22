
import { Component, ChangeDetectionStrategy, inject, computed, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="flex h-screen bg-gradient-to-br from-slate-50 to-cyan-50/30 overflow-hidden">
      <!-- Fixed Sidebar -->
      <aside class="w-64 flex flex-col flex-shrink-0 transition-all duration-300 {{ themeConfig().sidebar }} text-white shadow-xl">
        <div class="h-16 flex items-center px-6 {{ themeConfig().sidebarHeader }} border-b border-white/10">
           <div class="w-10 h-10 rounded-xl flex items-center justify-center text-white font-bold text-base mr-3 {{ themeConfig().logo }} shadow-md">H</div>
           <span class="text-base font-bold tracking-tight">HMS Core</span>
        </div>

        <nav class="flex-1 px-3 py-6 space-y-1 overflow-y-auto">
          @if (role() === 'Receptionist') {
            <!-- Receptionist Links -->
            <a routerLink="/app/dashboard" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z" /></svg>
              Dashboard
            </a>
            <a routerLink="/app/patients" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
              Patient Registry
            </a>
            <a routerLink="/app/schedule" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
              Schedule
            </a>
            <a routerLink="/app/billing" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 14l6-6m-5.5.5h.01m4.99 5h.01M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16l3.5-2 3.5 2 3.5-2 3.5 2zM10 8.5a.5.5 0 11-1 0 .5.5 0 011 0zm5 5a.5.5 0 11-1 0 .5.5 0 011 0z" /></svg>
               Billing & Payments
            </a>
          } @else if (role() === 'Nurse') {
            <!-- Nurse Links -->
            <a routerLink="/app/dashboard" routerLinkActive="bg-emerald-500/20 text-white border-l-4 border-emerald-400" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-emerald-50 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-emerald-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
               Triage Station
            </a>
            <a routerLink="/app/nurse-patients" routerLinkActive="bg-emerald-500/20 text-white border-l-4 border-emerald-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-emerald-50 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-emerald-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
               My Patient List
            </a>
            <a routerLink="/app/mar" routerLinkActive="bg-emerald-500/20 text-white border-l-4 border-emerald-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-emerald-50 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-emerald-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.384-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" /></svg>
               Medication Admin
            </a>
            <a routerLink="/app/labs" routerLinkActive="bg-emerald-500/20 text-white border-l-4 border-emerald-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-emerald-50 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-emerald-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.384-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" /></svg>
               Lab Requests
            </a>
          } @else if (role() === 'Doctor') {
             <!-- Doctor Links -->
            <a routerLink="/app/doctor-dashboard" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" /></svg>
              My Queue
            </a>
            <a routerLink="/app/patients" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
              All Patients
            </a>
            <a routerLink="/app/schedule" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
              <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
              On-Call Schedule
            </a>
             <a routerLink="/app/doctor-dashboard" routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
               Clinical Reports
            </a>
          } @else if (role() === 'Admin') {
            <!-- Admin Links -->
             <a routerLink="/app/admin/dashboard" routerLinkActive="bg-indigo-500/20 text-white border-l-4 border-indigo-400" [routerLinkActiveOptions]="{exact: true}" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-indigo-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
               System Pulse
            </a>
             <a routerLink="/app/admin/users" routerLinkActive="bg-indigo-500/20 text-white border-l-4 border-indigo-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-indigo-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
               User Management
            </a>
             <a routerLink="/app/admin/forms" routerLinkActive="bg-indigo-500/20 text-white border-l-4 border-indigo-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-indigo-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" /></svg>
               Form Builder
            </a>
             <a routerLink="/app/admin/audit" routerLinkActive="bg-indigo-500/20 text-white border-l-4 border-indigo-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-indigo-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
               Audit Logs
            </a>
             <a routerLink="/app/admin/health" routerLinkActive="bg-indigo-500/20 text-white border-l-4 border-indigo-400" class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg text-slate-200 hover:bg-white/10 hover:text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-indigo-400">
               <svg class="mr-3 h-5 w-5 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h14M5 12a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v4a2 2 0 01-2 2M5 12a2 2 0 00-2 2v4a2 2 0 002 2h14a2 2 0 002-2v-4a2 2 0 00-2-2m-2-4h.01M17 16h.01" /></svg>
               Infrastructure
            </a>
          }
        </nav>

        <div class="p-4 {{ themeConfig().sidebarHeader }} border-t border-white/10">
          <button (click)="auth.logout()" class="w-full flex items-center px-4 py-3 text-sm font-medium text-slate-300 hover:text-white hover:bg-white/10 transition-all duration-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2">
            <svg class="mr-3 h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" /></svg>
            Sign Out
          </button>
        </div>
      </aside>

      <!-- Main Content Area -->
      <div class="flex-1 flex flex-col min-w-0 overflow-hidden">
        <!-- Global Header -->
        <header class="h-16 bg-white shadow-md flex items-center justify-between px-8 z-10 border-b border-cyan-100">
          
          <!-- Global Search (Different for Admin) -->
          <div class="flex-1 max-w-2xl flex items-center gap-4">
            <div class="relative flex-1">
              <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" /></svg>
              </div>
              @if(role() === 'Admin') {
                <input type="text" class="block w-full pl-11 pr-4 py-2.5 border border-slate-300 rounded-lg leading-5 bg-slate-50 placeholder-slate-500 focus:outline-none focus:bg-white focus:ring-2 focus:ring-indigo-500 focus:border-transparent sm:text-sm transition-all duration-200" placeholder="Search Settings, Users, or Logs... (Ctrl+K)">
              } @else {
                <input type="text" class="block w-full pl-11 pr-4 py-2.5 border border-slate-300 rounded-lg leading-5 bg-slate-50 placeholder-slate-500 focus:outline-none focus:bg-white focus:ring-2 focus:ring-cyan-500 focus:border-transparent sm:text-sm transition-all duration-200" placeholder="Search Patient by Name, MRN, or Phone... (Ctrl+K)">
              }
            </div>
            
            <!-- Admin System Health Indicator -->
            @if (role() === 'Admin') {
              <div class="flex items-center gap-2 px-3 py-1.5 bg-emerald-50 text-emerald-700 rounded-full text-xs font-semibold border border-emerald-200 cursor-pointer hover:bg-emerald-100 transition-colors duration-200">
                 <span class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></span>
                 System Healthy
              </div>
            }

            <!-- Nurse Alert Ticker -->
            @if (role() === 'Nurse') {
               <div class="hidden lg:flex bg-red-50 text-red-700 px-3 py-1.5 rounded-full text-xs font-semibold items-center gap-2 animate-pulse cursor-pointer border border-red-200 hover:bg-red-100 transition-colors duration-200">
                  <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                  CRITICAL: Lab Value - Ross, Michael (K+ 6.5)
               </div>
            }
          </div>

          <!-- Right Actions -->
          <div class="ml-6 flex items-center space-x-6">
            
            <!-- Doctor Results Badge -->
            @if (role() === 'Doctor') {
               <button class="relative p-2 text-slate-500 hover:text-cyan-600 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-cyan-400 rounded-lg">
                  <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" /></svg>
                  <span class="absolute top-0 right-0 block h-5 w-5 rounded-full bg-red-500 text-white text-[11px] font-bold flex items-center justify-center border-2 border-white">{{ inboxCount() }}</span>
               </button>
            }

            <!-- Role Switcher -->
            <div class="relative">
              <button (click)="toggleRole()" class="flex items-center space-x-2 text-sm font-semibold text-slate-700 hover:text-cyan-600 focus:outline-none focus:ring-2 focus:ring-cyan-400 rounded px-2 py-1 transition-colors duration-200">
                <span class="py-1 px-3 rounded-full text-xs font-bold uppercase tracking-wide {{ themeConfig().roleBadge }}">
                  {{ role() }}
                </span>
              </button>
            </div>

            <!-- Profile -->
            <div class="flex items-center space-x-3 border-l border-slate-300 pl-6">
              <div class="h-10 w-10 rounded-full bg-gradient-to-br from-cyan-500 to-teal-600 flex items-center justify-center text-white font-bold text-sm shadow-md">
                 {{ auth.currentUser().name.substring(0,2).toUpperCase() }}
              </div>
            </div>
          </div>
        </header>

        <!-- Main Scrollable Area -->
        <main class="flex-1 overflow-auto bg-gradient-to-br from-slate-50 via-white to-cyan-50/40 px-8 py-6">
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
          sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900', 
          sidebarHeader: 'bg-slate-950', 
          logo: 'bg-gradient-to-br from-cyan-500 to-teal-600',
          roleBadge: 'bg-cyan-100 text-cyan-800'
        };
      case 'Nurse':
        return { 
          sidebar: 'bg-gradient-to-b from-emerald-800 to-emerald-900', 
          sidebarHeader: 'bg-emerald-950', 
          logo: 'bg-gradient-to-br from-emerald-500 to-green-600',
          roleBadge: 'bg-emerald-100 text-emerald-800'
        };
      case 'Doctor':
        return { 
          sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900', 
          sidebarHeader: 'bg-slate-950', 
          logo: 'bg-gradient-to-br from-cyan-500 to-blue-600',
          roleBadge: 'bg-cyan-100 text-cyan-800'
        };
      case 'Admin':
        return { 
          sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900', 
          sidebarHeader: 'bg-slate-950', 
          logo: 'bg-gradient-to-br from-indigo-500 to-purple-600',
          roleBadge: 'bg-indigo-100 text-indigo-800'
        };
      default:
        return { sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900', sidebarHeader: 'bg-slate-950', logo: 'bg-gradient-to-br from-cyan-500 to-teal-600', roleBadge: 'bg-cyan-100 text-cyan-800' };
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
