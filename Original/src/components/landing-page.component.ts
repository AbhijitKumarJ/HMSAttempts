
import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="min-h-screen bg-white font-sans">
      <!-- Sticky Header -->
      <header class="sticky top-0 z-50 bg-white/90 backdrop-blur-md border-b border-slate-200">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex justify-between items-center h-16">
            <!-- Logo -->
            <div class="flex items-center gap-2">
              <div class="w-8 h-8 bg-blue-600 rounded-lg flex items-center justify-center text-white font-bold text-xl">H</div>
              <span class="text-xl font-bold text-slate-900 tracking-tight">HMS Core</span>
            </div>
            
            <!-- Nav -->
            <nav class="hidden md:flex space-x-8">
              <a href="#" class="text-sm font-medium text-slate-600 hover:text-blue-600 transition-colors">Features</a>
              <a href="#" class="text-sm font-medium text-slate-600 hover:text-blue-600 transition-colors">Technology</a>
              <a href="#" class="text-sm font-medium text-slate-600 hover:text-blue-600 transition-colors">Support</a>
            </nav>

            <!-- Action -->
            <a routerLink="/login" class="inline-flex items-center justify-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-all shadow-sm">
              Login to Workspace
            </a>
          </div>
        </div>
      </header>

      <!-- Hero Section -->
      <main>
        <div class="relative pt-16 pb-32 flex content-center items-center justify-center min-h-[80vh]">
          <div class="absolute top-0 w-full h-full bg-center bg-cover" style="background-image: url('https://images.unsplash.com/photo-1519494026892-80bbd2d6fd0d?ixlib=rb-1.2.1&auto=format&fit=crop&w=1950&q=80'); opacity: 0.1;"></div>
          <div class="container relative mx-auto">
            <div class="items-center flex flex-wrap">
              <div class="w-full lg:w-8/12 px-4 ml-auto mr-auto text-center">
                <div class="inline-block p-2 rounded-full bg-blue-50 border border-blue-100 mb-6">
                  <span class="text-blue-600 font-semibold text-xs uppercase tracking-wide px-2">Enterprise Release v20.0</span>
                </div>
                <h1 class="text-5xl font-bold text-slate-900 leading-tight mb-6">
                  Democratizing <span class="text-blue-600">Enterprise-Grade</span> Healthcare.
                </h1>
                <p class="mt-4 text-lg text-slate-600 mb-10 max-w-2xl mx-auto">
                  Operational efficiency for the modern clinic. Streamline patient intake, manage provider schedules, and accelerate care delivery with HMS Core.
                </p>
                <a routerLink="/login" class="get-started text-white font-bold px-8 py-4 rounded-lg outline-none focus:outline-none mr-1 mb-1 bg-blue-600 active:bg-blue-700 uppercase text-sm shadow hover:shadow-lg ease-linear transition-all duration-150 inline-block">
                  Access Clinical Portal
                </a>
              </div>
            </div>
          </div>
        </div>

        <!-- Features Grid (Visual Filler) -->
        <section class="pb-20 bg-slate-50 -mt-24 pt-10">
          <div class="container mx-auto px-4">
            <div class="flex flex-wrap justify-center">
              
              <div class="lg:pt-12 pt-6 w-full md:w-4/12 px-4 text-center">
                <div class="relative flex flex-col min-w-0 break-words bg-white w-full mb-8 shadow-lg rounded-lg p-8">
                  <div class="px-4 py-5 flex-auto">
                    <div class="text-white p-3 text-center inline-flex items-center justify-center w-12 h-12 mb-5 shadow-lg rounded-full bg-emerald-500">
                      <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                    </div>
                    <h6 class="text-xl font-semibold text-slate-800">Real-time Queue</h6>
                    <p class="mt-2 mb-4 text-slate-500">
                      Monitor patient flow instantly. Reduce wait times with intelligent triage indicators.
                    </p>
                  </div>
                </div>
              </div>

              <div class="w-full md:w-4/12 px-4 text-center">
                <div class="relative flex flex-col min-w-0 break-words bg-white w-full mb-8 shadow-lg rounded-lg p-8">
                  <div class="px-4 py-5 flex-auto">
                    <div class="text-white p-3 text-center inline-flex items-center justify-center w-12 h-12 mb-5 shadow-lg rounded-full bg-blue-500">
                      <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
                    </div>
                    <h6 class="text-xl font-semibold text-slate-800">Digital Records</h6>
                    <p class="mt-2 mb-4 text-slate-500">
                      Secure, instant access to patient history, insurance, and billing data.
                    </p>
                  </div>
                </div>
              </div>

            </div>
          </div>
        </section>
      </main>

      <!-- Footer -->
      <footer class="bg-slate-900 text-slate-400 py-8">
        <div class="max-w-7xl mx-auto px-4 flex flex-col md:flex-row justify-between items-center">
          <div class="mb-4 md:mb-0">
            <span class="font-semibold text-slate-200">HMS Core</span> &copy; 2024
          </div>
          <div class="flex items-center space-x-6">
            <div class="flex items-center gap-2">
              <span class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></span>
              <span class="text-sm">Systems Operational</span>
            </div>
            <a href="#" class="text-sm hover:text-white transition-colors">IT Support</a>
            <a href="#" class="text-sm hover:text-white transition-colors">Privacy</a>
          </div>
        </div>
      </footer>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LandingPageComponent {}
