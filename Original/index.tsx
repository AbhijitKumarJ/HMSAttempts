

import { bootstrapApplication } from '@angular/platform-browser';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withHashLocation, Routes } from '@angular/router';
import { AppComponent } from './src/app.component';
import { LandingPageComponent } from './src/components/landing-page.component';
import { LoginComponent } from './src/components/login.component';
import { MainLayoutComponent } from './src/components/main-layout.component';
import { DashboardComponent } from './src/components/dashboard.component';
import { PatientRegistryComponent } from './src/components/patient-registry.component';
import { PatientDetailComponent } from './src/components/patient-detail.component';
import { ScheduleComponent } from './src/components/schedule.component';
import { BookingComponent } from './src/components/booking.component';
import { BillingComponent } from './src/components/billing.component';
import { NursePatientListComponent } from './src/components/nurse-patient-list.component';
import { TriageComponent } from './src/components/triage.component';
import { MarComponent } from './src/components/mar.component';
import { DoctorDashboardComponent } from './src/components/doctor-dashboard.component';
import { ClinicalCockpitComponent } from './src/components/clinical-cockpit.component';
import { AdminDashboardComponent } from './src/components/admin-dashboard.component';
import { AdminUsersComponent } from './src/components/admin-users.component';
import { AdminFormsComponent } from './src/components/admin-forms.component';
import { AdminAuditComponent } from './src/components/admin-audit.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { 
    path: 'app', 
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      
      // Receptionist Routes
      { path: 'patients', component: PatientRegistryComponent },
      { path: 'patients/:mrn', component: PatientDetailComponent },
      { path: 'schedule', component: ScheduleComponent },
      { path: 'schedule/book', component: BookingComponent },
      { path: 'billing', component: BillingComponent },
      
      // Nurse Routes
      { path: 'nurse-patients', component: NursePatientListComponent },
      { path: 'triage/:mrn', component: TriageComponent },
      { path: 'mar', component: MarComponent },
      { path: 'mar/:mrn/:orderId', component: MarComponent },
      { path: 'labs', component: MarComponent },
      
      // Doctor Routes
      { path: 'doctor-dashboard', component: DoctorDashboardComponent },
      { path: 'clinical/cockpit/:mrn', component: ClinicalCockpitComponent },
      { path: 'reports', component: BillingComponent }, 
      
      // Admin Routes
      { path: 'admin/dashboard', component: AdminDashboardComponent },
      { path: 'admin/users', component: AdminUsersComponent },
      { path: 'admin/forms', component: AdminFormsComponent },
      { path: 'admin/audit', component: AdminAuditComponent },
      { path: 'admin/settings', component: AdminDashboardComponent }, // Placeholder
      { path: 'admin/health', component: AdminDashboardComponent }, // Placeholder

      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: '' }
];

bootstrapApplication(AppComponent, {
  providers: [
    provideZonelessChangeDetection(),
    provideRouter(routes, withHashLocation())
  ]
}).catch((err) => console.error(err));

// AI Studio always uses an `index.tsx` file for all project types.
