import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageuploadfieldComponent } from './imageuploadfield.component';

describe('ImageuploadfieldComponent', () => {
  let component: ImageuploadfieldComponent;
  let fixture: ComponentFixture<ImageuploadfieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImageuploadfieldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ImageuploadfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
