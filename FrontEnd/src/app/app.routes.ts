import { Routes } from '@angular/router';
import { DashboardComponent } from './component/dashboard/dashboard.component';
import { HistoryComponent } from './component/history/history.component';
import { ComingSoonComponent } from './assets/coming-soon/coming-soon.component';
import { NotFoundComponent } from './assets/not-found/not-found.component';

export const routes: Routes = [
    { path: 'dashboard', component: DashboardComponent },
    { path: 'history', component: HistoryComponent },
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'coming-soon', component: ComingSoonComponent },
    { path: 'not-found', component: NotFoundComponent }
];
