import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadConversionReport } from './lead-conversion-report';

describe('LeadConversionReport', () => {
  let component: LeadConversionReport;
  let fixture: ComponentFixture<LeadConversionReport>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeadConversionReport]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeadConversionReport);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
