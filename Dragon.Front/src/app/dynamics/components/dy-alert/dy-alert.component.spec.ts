import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DyAlertComponent } from './dy-alert.component';

describe('DyAlertComponent', () => {
  let component: DyAlertComponent;
  let fixture: ComponentFixture<DyAlertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DyAlertComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DyAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
