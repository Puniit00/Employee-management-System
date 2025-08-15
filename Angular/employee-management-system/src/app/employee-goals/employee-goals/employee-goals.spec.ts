import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGoals } from './employee-goals';

describe('EmployeeGoals', () => {
  let component: EmployeeGoals;
  let fixture: ComponentFixture<EmployeeGoals>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EmployeeGoals]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeGoals);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
