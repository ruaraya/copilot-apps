import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpportunityList } from './opportunity-list';

describe('OpportunityList', () => {
  let component: OpportunityList;
  let fixture: ComponentFixture<OpportunityList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OpportunityList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OpportunityList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
