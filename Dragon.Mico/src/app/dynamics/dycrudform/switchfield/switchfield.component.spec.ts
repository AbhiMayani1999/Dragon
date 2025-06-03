import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SwitchfieldComponent } from './switchfield.component';

describe('SwitchfieldComponent', () => {
  let component: SwitchfieldComponent;
  let fixture: ComponentFixture<SwitchfieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SwitchfieldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SwitchfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
