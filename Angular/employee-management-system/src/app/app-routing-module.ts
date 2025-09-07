import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { EmployeeList } from './employee-list/employee-list';
// import { AddEmployeeComponent } from './add-amployee/add-employee';
import { UpdateEmployeeComponent } from './update-employee/update-employee';
import { EmployeeGoalsComponent } from './employee-goals/employee-goals/employee-goals';
import { AddGoalsComponent } from './employee-goals/add-goals/add-goals';
import { PerformanceReviewComponent } from './review/performance-review/performance-review';
import { LoginComponent } from './login/login.component/login.component';
import { SignupComponent } from './login/signup.component/signup.component';

const routes: Routes = [
  { path: '', component: LoginComponent }, // default route to login
  // { path: 'employee', component: EmployeeList }, 
  { 
    path: 'add-employee', 
    // ❌ loadChildren is for NgModules, not for components. Since AddEmployeeComponent is not standalone
    // and is declared in AppModule, it cannot be lazy-loaded with loadChildren.
    // ✅ If AddEmployeeComponent is converted to standalone, then loadComponent should be used instead.
    loadChildren: () => import('./add-amployee/add-employee').then((c) => c.AddEmployeeComponent) 
  },
  { path: 'update-employee', component: UpdateEmployeeComponent },
  { path: 'employee-goals/:id', component: EmployeeGoalsComponent },
  { path: 'performance-review/:id', component: PerformanceReviewComponent },
  { path: 'add-goal/:id', component: AddGoalsComponent },
  { path: 'signup', component: SignupComponent },
  { 
    path: 'employee', 
    // ✅ EmployeeList has been converted to a standalone component, 
    // so it can be lazy-loaded directly using loadComponent.
    loadComponent: () => import('./employee-list/employee-list').then(c => c.EmployeeList) 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
