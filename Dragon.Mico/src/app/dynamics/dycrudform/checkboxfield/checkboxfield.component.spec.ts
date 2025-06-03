import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckboxfieldComponent } from './checkboxfield.component';

describe('CheckboxfieldComponent', () => {
  let component: CheckboxfieldComponent;
  let fixture: ComponentFixture<CheckboxfieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckboxfieldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CheckboxfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
