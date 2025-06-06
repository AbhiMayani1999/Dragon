import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicsComponent } from './dynamics.component';

describe('DynamicsComponent', () => {
  let component: DynamicsComponent;
  let fixture: ComponentFixture<DynamicsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DynamicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
