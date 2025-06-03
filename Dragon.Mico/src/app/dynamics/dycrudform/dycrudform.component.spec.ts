import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DycrudformComponent } from './dycrudform.component';

describe('DycrudformComponent', () => {
  let component: DycrudformComponent;
  let fixture: ComponentFixture<DycrudformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DycrudformComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DycrudformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
