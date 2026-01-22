
import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-slate-50 via-cyan-50 to-emerald-50 font-sans">
      <!-- Sticky Header -->
      <header class="sticky top-0 z-50 bg-white/95 backdrop-blur-lg border-b border-cyan-100 shadow-sm">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex justify-between items-center h-16">
            <!-- Logo -->
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 bg-gradient-to-br from-cyan-500 to-teal-600 rounded-xl flex items-center justify-center text-white font-bold text-lg shadow-md">H</div>
              <span class="text-xl font-bold text-slate-900 tracking-tight">HMS Core</span>
            </div>
            
            <!-- Nav -->
            <nav class="hidden md:flex space-x-10">
              <a href="#" class="text-sm font-medium text-slate-700 hover:text-cyan-600 transition-colors duration-200">Features</a>
              <a href="#" class="text-sm font-medium text-slate-700 hover:text-cyan-600 transition-colors duration-200">Technology</a>
              <a href="#" class="text-sm font-medium text-slate-700 hover:text-cyan-600 transition-colors duration-200">Support</a>
            </nav>

            <!-- Action -->
            <a routerLink="/login" class="inline-flex items-center justify-center px-5 py-2.5 border border-transparent text-sm font-semibold rounded-lg text-white bg-gradient-to-r from-cyan-500 to-teal-600 hover:from-cyan-600 hover:to-teal-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-400 transition-all duration-200 shadow-md hover:shadow-lg">
              Login to Workspace
            </a>
          </div>
        </div>
      </header>

      <!-- Hero Section -->
      <main>
        <div class="relative pt-20 pb-32 flex content-center items-center justify-center min-h-[75vh]">
          <div class="absolute inset-0 bg-gradient-to-br from-cyan-200/30 via-transparent to-emerald-200/30 pointer-events-none"></div>
          <div class="container relative mx-auto">
            <div class="items-center flex flex-wrap">
              <div class="w-full lg:w-8/12 px-4 ml-auto mr-auto text-center">
                <div class="inline-block p-2.5 rounded-full bg-cyan-100 border border-cyan-300 mb-8">
                  <span class="text-cyan-700 font-semibold text-xs uppercase tracking-widest px-3">✨ Enterprise Healthcare Platform</span>
                </div>
                <h1 class="text-5xl sm:text-6xl font-bold text-slate-900 leading-tight mb-6">
                  Modern Healthcare<br/><span class="bg-gradient-to-r from-cyan-500 to-emerald-500 bg-clip-text text-transparent">Management Simplified</span>
                </h1>
                <p class="mt-6 text-lg text-slate-600 mb-12 max-w-2xl mx-auto leading-relaxed">
                  Clinical-grade operational efficiency for modern healthcare facilities. Streamline patient intake, optimize provider schedules, and deliver exceptional care with HMS Core.
                </p>
                <a routerLink="/login" class="get-started text-white font-semibold px-8 py-3.5 rounded-xl outline-none focus:outline-none mr-2 mb-2 bg-gradient-to-r from-cyan-500 to-teal-600 hover:from-cyan-600 hover:to-teal-700 uppercase text-sm shadow-lg hover:shadow-xl ease-in-out transition-all duration-200 inline-block">
                  Access Clinical Portal
                </a>
              </div>
            </div>
          </div>
        </div>

        <!-- Features Grid (Visual Filler) -->
        <section class="pb-24 bg-white -mt-20 pt-20 relative z-10 shadow-lg shadow-cyan-100/20">
          <div class="container mx-auto px-4">
            <div class="text-center mb-16">
              <h2 class="text-3xl font-bold text-slate-900 mb-4">Trusted by Healthcare Leaders</h2>
              <p class="text-slate-600 max-w-2xl mx-auto">Purpose-built features designed by clinicians, for clinicians</p>
            </div>
            <div class="flex flex-wrap justify-center gap-8">
              
              <div class="lg:pt-0 pt-6 w-full md:w-5/12 lg:w-4/12 px-4">
                <div class="relative flex flex-col min-w-0 break-words bg-gradient-to-br from-slate-50 to-cyan-50 w-full mb-6 shadow-md rounded-2xl p-8 border border-cyan-100 hover:shadow-lg hover:border-cyan-200 transition-all duration-300">
                  <div class="px-0 py-0 flex-auto">
                    <div class="text-white p-3 text-center inline-flex items-center justify-center w-14 h-14 mb-5 shadow-lg rounded-full bg-gradient-to-br from-cyan-400 to-teal-500">
                      <svg class="w-7 h-7" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                    </div>
                    <h6 class="text-lg font-bold text-slate-900">Real-time Queue</h6>
                    <p class="mt-3 mb-0 text-slate-600 leading-relaxed">
                      Monitor patient flow instantly. Reduce wait times with intelligent triage indicators and predictive scheduling.
                    </p>
                  </div>
                </div>
              </div>

              <div class="w-full md:w-5/12 lg:w-4/12 px-4">
                <div class="relative flex flex-col min-w-0 break-words bg-gradient-to-br from-slate-50 to-emerald-50 w-full mb-6 shadow-md rounded-2xl p-8 border border-emerald-100 hover:shadow-lg hover:border-emerald-200 transition-all duration-300">
                  <div class="px-0 py-0 flex-auto">
                    <div class="text-white p-3 text-center inline-flex items-center justify-center w-14 h-14 mb-5 shadow-lg rounded-full bg-gradient-to-br from-emerald-400 to-green-500">
                      <svg class="w-7 h-7" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                    </div>
                    <h6 class="text-lg font-bold text-slate-900">Digital Records</h6>
                    <p class="mt-3 mb-0 text-slate-600 leading-relaxed">
                      Secure, instant access to complete patient history, insurance data, and billing information in one place.
                    </p>
                  </div>
                </div>
              </div>

            </div>
          </div>
        </section>
      </main>

      <!-- Footer -->
      <footer class="bg-gradient-to-r from-slate-900 to-slate-800 text-slate-300 py-10 border-t border-slate-700">
        <div class="max-w-7xl mx-auto px-4 flex flex-col md:flex-row justify-between items-center">
          <div class="mb-4 md:mb-0 flex items-center gap-3">
            <div class="w-8 h-8 bg-gradient-to-br from-cyan-400 to-teal-500 rounded-lg flex items-center justify-center text-white font-bold text-sm"></div>
            <span class="font-semibold text-slate-200">HMS Core</span> &copy; 2024
          </div>
          <div class="flex items-center space-x-8">
            <div class="flex items-center gap-2">
              <span class="w-2.5 h-2.5 rounded-full bg-emerald-400 animate-pulse"></span>
              <span class="text-sm text-slate-400">Systems Operational</span>
            </div>
            <a href="#" class="text-sm text-slate-400 hover:text-slate-200 transition-colors duration-200">Support</a>
            <a href="#" class="text-sm text-slate-400 hover:text-slate-200 transition-colors duration-200">Privacy</a>
          </div>
        </div>
      </footer>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LandingPageComponent {}
