import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { LeadsModule } from './leads/leads-module';
import { OpportunitiesModule } from './opportunities/opportunities-module';
import { ContactsModule } from './contacts/contacts-module';
import { ReportsModule } from './reports/reports-module';
import { AdminModule } from './admin/admin-module';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'leads', component: LeadsModule, canActivate: [AuthGuard] },
  { path: 'opportunities', component: OpportunitiesModule, canActivate: [AuthGuard] },
  { path: 'contacts', component: ContactsModule, canActivate: [AuthGuard] },
  { path: 'reports', component: ReportsModule, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminModule, canActivate: [AuthGuard] },
  //{ path: 'login', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}