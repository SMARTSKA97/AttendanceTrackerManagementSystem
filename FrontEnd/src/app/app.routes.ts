import { Routes } from '@angular/router';
import { ComingSoonComponent } from './general/coming-soon/coming-soon.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { NotFoundComponent } from './general/not-found/not-found.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const routes: Routes = [
    { path: 'dashboard', component: DashboardComponent},
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'coming-soon', component: ComingSoonComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'statistics', component: StatisticsComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
];
