import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformanceReview } from './performance-review';

describe('PerformanceReview', () => {
  let component: PerformanceReview;
  let fixture: ComponentFixture<PerformanceReview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PerformanceReview]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerformanceReview);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
