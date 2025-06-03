import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DycrudtableComponent } from './dycrudtable.component';

describe('DycrudtableComponent', () => {
  let component: DycrudtableComponent;
  let fixture: ComponentFixture<DycrudtableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DycrudtableComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DycrudtableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
