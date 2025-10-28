import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpportunityEdit } from './opportunity-edit';

describe('OpportunityEdit', () => {
  let component: OpportunityEdit;
  let fixture: ComponentFixture<OpportunityEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OpportunityEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OpportunityEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
