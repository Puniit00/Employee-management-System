import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAmployee } from './add-amployee';

describe('AddAmployee', () => {
  let component: AddAmployee;
  let fixture: ComponentFixture<AddAmployee>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddAmployee]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAmployee);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
