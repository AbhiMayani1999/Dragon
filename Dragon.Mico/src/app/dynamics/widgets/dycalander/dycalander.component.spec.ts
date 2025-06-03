import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DycalanderComponent } from './dycalander.component';

describe('DycalanderComponent', () => {
  let component: DycalanderComponent;
  let fixture: ComponentFixture<DycalanderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DycalanderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DycalanderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
