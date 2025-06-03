import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpEventType } from '@angular/common/http';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { DROPZONE_CONFIG, DropzoneComponent, DropzoneConfigInterface, DropzoneModule } from 'ngx-dropzone-wrapper';
import { FileIconComponent } from '../file-icon/file-icon.component';
import { FileContentTypes } from '../upload.model';
import { FormFieldConfig } from '../../dycrudfrom.model';
import { DomSanitizer } from '@angular/platform-browser';
import { DataService } from '@core/services/data.service';
import { Modules, RouteExtensions } from '@urls';
import { Subscription } from 'rxjs';
import { ApiResponse, StatusFlags, eMessageType } from '@core/models/data.model';

const DzFileConfig: DropzoneConfigInterface = {
  url: 'https://abc.com/',
  thumbnailWidth: 160,
  autoProcessQueue: false,
  acceptedFiles: FileContentTypes.File
};

@Component({
  selector: 'app-fileuploadfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DropzoneModule, HttpClientModule, FileIconComponent],
  providers: [{ provide: DROPZONE_CONFIG, useValue: DzFileConfig }],
  templateUrl: './fileuploadfield.component.html',
  styleUrl: './fileuploadfield.component.scss'
})
export class FileuploadfieldComponent {
  public fieldConfig: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;

  @ViewChild('dropzones') dropzone: DropzoneComponent;
  public dzConfig: DropzoneConfigInterface = { ...DzFileConfig };

  private isMultiple = false;
  public fileList: string[] = [];
  public existingFiles: string[] = [];

  constructor(public sanitizer: DomSanitizer, public dataService: DataService, private changeDetector: ChangeDetectorRef) { }

  public set config(data: FormFieldConfig) {
    if (!this.fieldConfig) {
      this.fieldConfig = data;
      this.existingFiles = [];
      setTimeout(() => {
        const existingFile = this.formGroup.value[this.fieldConfig.jsonProperty];
        if (this.isMultiple) { this.existingFiles.concat(existingFile) } else { if (existingFile) { this.existingFiles = [existingFile]; } }
        this.changeDetector.detectChanges();
      }, 100);
    }
  }

  onUpload(files: any): void {
    this.dropzone.directiveRef.reset();
    if (files.length > 1 && !this.isMultiple) { this.dataService.notify.next({ key: eMessageType.Error, value: 'Upload single file' }); return; }
    for (let i = 0; i < files.length; i++) {
      if (!this.isMultiple) { this.fileList = []; }
      const file = files[i];
      this.fileList.push(file.name);
      const postFileSubscription: Subscription = this.dataService.postFile(Modules.Files.Tempfile, file)
        .subscribe(
          {
            next: (response) => {
              if (response.type === HttpEventType.Response) {
                const apiResponse: any = this.dataService.completeResponse<ApiResponse<string>>(response.body);
                if (!this.isMultiple) { this.fileList = [apiResponse.data]; } else { this.fileList.filter(d => d == file.name)[0] = apiResponse.data; }
              }
              this.fillPropertyValue();
            },
            error: (error) => this.dataService.notify.next({ key: eMessageType.Error, value: error }),
            complete: () => postFileSubscription.unsubscribe()
          }
        );
    }
  }

  fillPropertyValue() {
    let valueToSet: string | string[] = this.fileList.length ? this.fileList[0] : '';
    if (this.isMultiple) {
      valueToSet = this.fileList.map(d => d);
      valueToSet = valueToSet.concat(...this.existingFiles);
    } else { this.existingFiles = []; }
    this.formGroup.controls[this.fieldConfig.jsonProperty].setValue(valueToSet);
  }

  downloadTempFile(filename: string) {
    this.dataService.downloadFile(`${Modules.Files.Tempfile}/${RouteExtensions.Download}/${filename}`, {}, filename);
  }

  downloadExistingFile(filename: string) {
    this.dataService.downloadFile(`${Modules.Files.Tenantfile}/${RouteExtensions.Download}/${filename}`, filename);
  }

  deleteTempFile(filename: string) {
    this.dataService
      .delete<any>(Modules.Files.Tempfile, filename)
      .then((response) => {
        if (response.status === StatusFlags.Success) {
          this.fileList = this.fileList.filter(d => d !== filename);
          this.fillPropertyValue();
        }
      });
  }

  deleteExistingFile(filename: string) {
    this.existingFiles = this.existingFiles.filter(d => d !== filename);
    this.fillPropertyValue();
  }
}
