import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGoals } from './add-goals';

describe('AddGoals', () => {
  let component: AddGoals;
  let fixture: ComponentFixture<AddGoals>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddGoals]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddGoals);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
