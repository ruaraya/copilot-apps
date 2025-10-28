import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadEdit } from './lead-edit';

describe('LeadEdit', () => {
  let component: LeadEdit;
  let fixture: ComponentFixture<LeadEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeadEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeadEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
