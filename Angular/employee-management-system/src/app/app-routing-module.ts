import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeList } from './employee-list/employee-list';
import { AddEmployeeComponent } from './add-amployee/add-employee';
import { UpdateEmployeeComponent } from './update-employee/update-employee';
import { EmployeeGoalsComponent } from './employee-goals/employee-goals/employee-goals';
import { AddGoalsComponent } from './employee-goals/add-goals/add-goals';
import { PerformanceReviewComponent } from './review/performance-review/performance-review';

const routes: Routes = [
  { path: '', component: EmployeeList },      // default route
  { path: 'add-employee', component: AddEmployeeComponent }, // add employee form
  { path: 'update-employee', component: UpdateEmployeeComponent }, // add employee form,
  { path: 'employee-goals/:id', component: EmployeeGoalsComponent }, // add employee form,
  { path: 'performance-review/:id', component: PerformanceReviewComponent }, // add employee form,
  { path: 'add-goal/:id', component: AddGoalsComponent } // add employee form,
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
