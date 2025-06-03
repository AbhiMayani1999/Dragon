import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DycruddashboardComponent } from './dycruddashboard.component';

describe('DycruddashboardComponent', () => {
  let component: DycruddashboardComponent;
  let fixture: ComponentFixture<DycruddashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DycruddashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DycruddashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
