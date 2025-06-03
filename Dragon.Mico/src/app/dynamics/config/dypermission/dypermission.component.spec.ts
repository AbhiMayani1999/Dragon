import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DypermissionComponent } from './dypermission.component';

describe('DypermissionComponent', () => {
  let component: DypermissionComponent;
  let fixture: ComponentFixture<DypermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DypermissionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DypermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
