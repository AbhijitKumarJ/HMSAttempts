
import { Component, ChangeDetectionStrategy, inject } from '@angular/core';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  template: `
    <div class="space-y-6">
      <h1 class="text-2xl font-bold text-slate-900">System Pulse</h1>
      
      <!-- Top Row: System Metrics -->
      <div class="grid grid-cols-1 md:grid-cols-4 gap-6">
         <!-- DB Status -->
         <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200">
            <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Database</div>
            <div class="mt-2 flex items-center gap-2">
               <span class="w-3 h-3 rounded-full bg-emerald-500"></span>
               <span class="text-xl font-bold text-slate-900">Connected</span>
            </div>
            <div class="mt-2 text-xs text-slate-500">Latency: 12ms</div>
         </div>

         <!-- Queue Depth -->
         <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200">
            <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Event Bus</div>
            <div class="mt-2 flex items-center gap-2">
               <span class="text-xl font-bold text-slate-900">0</span>
               <span class="text-xs text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded">Optimal</span>
            </div>
            <div class="mt-2 text-xs text-slate-500">Messages Pending</div>
         </div>

         <!-- Workers -->
         <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200">
            <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Background Workers</div>
            <div class="mt-2 flex items-center gap-2">
               <span class="text-xl font-bold text-slate-900">4 / 4</span>
               <span class="text-xs text-slate-400">Running</span>
            </div>
            <button class="mt-2 text-xs text-indigo-600 font-bold hover:underline">Restart Pool</button>
         </div>

         <!-- Active Sessions -->
         <div class="bg-white rounded-xl p-6 shadow-sm border border-slate-200">
            <div class="text-sm font-medium text-slate-500 uppercase tracking-wider">Active Sessions</div>
            <div class="mt-2 flex items-center gap-2">
               <span class="text-xl font-bold text-slate-900">142</span>
            </div>
            <div class="mt-2 text-xs text-slate-500">Peak: 156 (1h ago)</div>
         </div>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
         <!-- Security Watch -->
         <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6">
            <div class="flex justify-between items-center mb-4">
               <h3 class="font-bold text-slate-800">Security Watch</h3>
               <button class="text-xs text-indigo-600 hover:text-indigo-800 font-medium">View All</button>
            </div>
            <div class="space-y-4">
               <div class="flex justify-between items-center p-3 bg-red-50 border border-red-100 rounded-lg">
                  <div class="flex items-center gap-3">
                     <div class="bg-red-100 p-2 rounded text-red-600">
                        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" /></svg>
                     </div>
                     <div>
                        <div class="text-sm font-bold text-slate-900">Failed Login Attempt</div>
                        <div class="text-xs text-slate-500">IP: 203.0.113.42 • User: admin</div>
                     </div>
                  </div>
                  <button class="text-xs bg-white border border-red-200 text-red-700 px-2 py-1 rounded hover:bg-red-100">Ban IP</button>
               </div>
                <div class="flex justify-between items-center p-3 bg-slate-50 border border-slate-100 rounded-lg">
                  <div class="flex items-center gap-3">
                     <div class="bg-blue-100 p-2 rounded text-blue-600">
                        <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" /></svg>
                     </div>
                     <div>
                        <div class="text-sm font-bold text-slate-900">Role Elevation</div>
                        <div class="text-xs text-slate-500">User 'dr_james' added to 'Admin'</div>
                     </div>
                  </div>
                  <span class="text-xs text-slate-400">10m ago</span>
               </div>
            </div>
         </div>

         <!-- Activity Chart Placeholder -->
         <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-6 flex flex-col justify-center items-center text-center">
             <div class="bg-indigo-50 p-4 rounded-full mb-4">
               <svg class="w-8 h-8 text-indigo-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 12l3-3 3 3 4-4M8 21l4-4 4 4M3 4h18M4 4h16v12a1 1 0 01-1 1H5a1 1 0 01-1-1V4z" /></svg>
             </div>
             <h3 class="font-bold text-slate-800">Activity Analytics</h3>
             <p class="text-sm text-slate-500 mt-2 max-w-xs">User activity visualization charts would be rendered here using D3.js in a production build.</p>
         </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminDashboardComponent {}
