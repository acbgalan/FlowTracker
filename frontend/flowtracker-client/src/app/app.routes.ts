import { Routes } from '@angular/router';
import { Dashboard } from './shared/components/dashboard/dashboard';
import { Categories } from './features/categories/categories';
import { Transactions } from './features/transactions/transactions';
import { SavingGoals } from './features/saving-goals/saving-goals';
import { Reports } from './features/reports/reports';

export const routes: Routes = [
    { path: 'home', component: Dashboard },
    { path: 'categories', component: Categories },
    { path: 'transactions', component: Transactions },
    { path: 'saving-goals', component: SavingGoals},
    { path: 'reports', component: Reports },
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: '**', redirectTo: 'home' }
];
