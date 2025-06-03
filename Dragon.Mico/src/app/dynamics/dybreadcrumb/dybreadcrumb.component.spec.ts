import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DybreadcrumbComponent } from './dybreadcrumb.component';

describe('DybreadcrumbComponent', () => {
  let component: DybreadcrumbComponent;
  let fixture: ComponentFixture<DybreadcrumbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DybreadcrumbComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DybreadcrumbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
