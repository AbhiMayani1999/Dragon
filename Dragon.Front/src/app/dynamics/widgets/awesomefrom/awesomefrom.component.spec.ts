import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AwesomefromComponent } from './awesomefrom.component';

describe('AwesomefromComponent', () => {
  let component: AwesomefromComponent;
  let fixture: ComponentFixture<AwesomefromComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AwesomefromComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AwesomefromComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
