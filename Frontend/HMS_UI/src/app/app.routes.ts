import { Routes } from '@angular/router';
import { LandingComponent } from './landing/landing.component';
import { LoginComponent } from './auth/login/login.component';
import { AdminDashboardComponent } from './dashboards/admin/admin-dashboard.component';
import { DoctorDashboardComponent } from './dashboards/doctor/doctor-dashboard.component';
import { NurseDashboardComponent } from './dashboards/nurse/nurse-dashboard.component';
import { ReceptionistDashboardComponent } from './dashboards/receptionist/receptionist-dashboard.component';
import { BillingDashboardComponent } from './dashboards/billing/billing-dashboard.component';
import { PatientDashboardComponent } from './dashboards/patient/patient-dashboard.component';

export const routes: Routes = [
    { path: '', component: LandingComponent },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard/admin', component: AdminDashboardComponent },
    { path: 'dashboard/doctor', component: DoctorDashboardComponent },
    { path: 'dashboard/nurse', component: NurseDashboardComponent },
    { path: 'dashboard/receptionist', component: ReceptionistDashboardComponent },
    { path: 'dashboard/billing', component: BillingDashboardComponent },
    { path: 'dashboard/patient', component: PatientDashboardComponent },
    { path: '**', redirectTo: '' }
];
