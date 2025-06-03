import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileuploadfieldComponent } from './fileuploadfield.component';

describe('FileuploadfieldComponent', () => {
  let component: FileuploadfieldComponent;
  let fixture: ComponentFixture<FileuploadfieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FileuploadfieldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FileuploadfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
