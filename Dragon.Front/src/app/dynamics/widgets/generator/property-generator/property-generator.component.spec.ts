import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyGeneratorComponent } from './property-generator.component';

describe('PropertyGeneratorComponent', () => {
  let component: PropertyGeneratorComponent;
  let fixture: ComponentFixture<PropertyGeneratorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PropertyGeneratorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PropertyGeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
