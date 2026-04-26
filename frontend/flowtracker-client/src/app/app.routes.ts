import { Routes } from '@angular/router';
import { Layout } from './shared/components/layout/layout';
import { Dashboard } from './shared/components/dashboard/dashboard';
import { Categories } from './features/categories/categories';
import { Transactions } from './features/transactions/transactions';
import { SavingGoals } from './features/saving-goals/saving-goals';
import { Reports } from './features/reports/reports';
import { Login } from './features/login/login';
import { Register } from './features/register/register';

export const routes: Routes = [
    // Routes without layout
    { path: 'login', component: Login },
    { path: 'register', component: Register},
    
    // Rutas with layout
    {
        path: '',
        component: Layout,
        children: [
            { path: 'home', component: Dashboard },
            { path: 'categories', component: Categories },
            { path: 'transactions', component: Transactions },
            { path: 'saving-goals', component: SavingGoals },
            { path: 'reports', component: Reports },
            { path: '', redirectTo: 'home', pathMatch: 'full' }
        ]
    },
    
    { path: '**', redirectTo: 'home' }
];
