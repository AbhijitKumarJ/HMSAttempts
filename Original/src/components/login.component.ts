
import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService, UserRole } from '../services/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-cyan-50 via-slate-50 to-emerald-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div class="sm:mx-auto sm:w-full sm:max-w-md">
        <div class="flex justify-center">
           <div class="w-14 h-14 bg-gradient-to-br from-cyan-500 to-teal-600 rounded-2xl flex items-center justify-center text-white font-bold text-2xl shadow-lg">H</div>
        </div>
        <h2 class="mt-8 text-center text-3xl font-bold text-slate-900">
          Welcome Back
        </h2>
        <p class="mt-2 text-center text-sm text-slate-600 leading-relaxed">
          Sign in to your secure clinical workspace
        </p>
      </div>

      <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-md">
        <div class="bg-white py-10 px-6 shadow-lg sm:rounded-2xl sm:px-12 border border-cyan-100">
          <form class="space-y-6" (ngSubmit)="handleLogin()">
            <div>
              <label for="email" class="block text-sm font-semibold text-slate-900">Username or Email</label>
              <div class="mt-2">
                <input id="email" name="email" type="text" [(ngModel)]="username" required autoFocus class="appearance-none block w-full px-4 py-2.5 border border-slate-300 rounded-lg shadow-sm placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent sm:text-sm transition-all duration-200">
              </div>
            </div>

            <div>
              <label for="password" class="block text-sm font-semibold text-slate-900">Password</label>
              <div class="mt-2 relative">
                <input [type]="showPassword() ? 'text' : 'password'" id="password" name="password" required class="appearance-none block w-full px-4 py-2.5 border border-slate-300 rounded-lg shadow-sm placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent sm:text-sm transition-all duration-200">
                <button type="button" (click)="togglePassword()" class="absolute inset-y-0 right-0 pr-4 flex items-center text-slate-500 hover:text-slate-700 transition-colors duration-200">
                   @if (showPassword()) {
                     <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" /></svg>
                   } @else {
                     <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" /></svg>
                   }
                </button>
              </div>
            </div>

             <!-- Role Selection Mock -->
            <div>
              <label for="role" class="block text-sm font-semibold text-slate-900">Role Context</label>
              <select id="role" [(ngModel)]="selectedRole" name="role" class="mt-2 block w-full px-4 py-2.5 text-base border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent sm:text-sm transition-all duration-200 bg-white">
                <option value="Receptionist">Receptionist (Admin)</option>
                <option value="Nurse">Nurse (Clinical)</option>
                <option value="Doctor">Doctor (Provider)</option>
                <option value="Admin">System Administrator</option>
              </select>
            </div>

            <div class="pt-2">
              <button type="submit" class="w-full flex justify-center py-2.5 px-4 border border-transparent rounded-lg shadow-md text-sm font-semibold text-white bg-gradient-to-r from-cyan-500 to-teal-600 hover:from-cyan-600 hover:to-teal-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-400 transition-all duration-200">
                Sign In
              </button>
            </div>
          </form>

          <div class="mt-8">
            <div class="relative">
              <div class="absolute inset-0 flex items-center">
                <div class="w-full border-t border-slate-200"></div>
              </div>
              <div class="relative flex justify-center text-sm">
                <span class="px-2 bg-white text-slate-500 font-medium">Emergency Access</span>
              </div>
            </div>

            <div class="mt-6">
              <button type="button" class="w-full inline-flex justify-center py-2.5 px-4 border-2 border-emerald-500 rounded-lg shadow-sm bg-white text-sm font-semibold text-emerald-700 hover:bg-emerald-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-emerald-400 transition-all duration-200">
                <span class="flex items-center gap-2">
                  <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>
                  Emergency Registration Access
                </span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {
  username = 'alex_staff';
  selectedRole: UserRole = 'Receptionist';
  showPassword = signal(false);

  constructor(private authService: AuthService) {}

  togglePassword() {
    this.showPassword.update(v => !v);
  }

  handleLogin() {
    this.authService.login(this.username, this.selectedRole);
  }
}
