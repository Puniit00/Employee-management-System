import {
  NgModule,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { EmployeeList } from './employee-list/employee-list';
import { AddEmployeeComponent } from './add-amployee/add-employee';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { UpdateEmployeeComponent } from './update-employee/update-employee';
import { EmployeeGoalsComponent } from './employee-goals/employee-goals/employee-goals';
import { AddGoalsComponent } from './employee-goals/add-goals/add-goals';
import { PerformanceReviewComponent } from './review/performance-review/performance-review';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component/login.component';
import { AuthInterceptor } from './auth.interceptor';
import { SignupComponent } from './login/signup.component/signup.component';
import { ScrollingModule } from '@angular/cdk/scrolling';

@NgModule({
  declarations: [
    App,
    EmployeeList,
    AddEmployeeComponent,
    UpdateEmployeeComponent,
    EmployeeGoalsComponent,
    AddGoalsComponent,
    PerformanceReviewComponent,
    HeaderComponent,
    LoginComponent,
    SignupComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    ScrollingModule,
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    DatePipe,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [App],
})
export class AppModule {}
