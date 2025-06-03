import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AwesomedatatableComponent } from './awesomedatatable.component';

describe('AwesomedatatableComponent', () => {
  let component: AwesomedatatableComponent;
  let fixture: ComponentFixture<AwesomedatatableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AwesomedatatableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AwesomedatatableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
