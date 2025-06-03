import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';

@Component({
  selector: 'dy-file-icon',
  standalone: true,
  imports: [],
  templateUrl: './file-icon.component.html',
  styleUrl: './file-icon.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FileIconComponent {
  basePath = '../../../../../assets/images/file/';
  avatarUrl = '';

  @Input() Height: number;

  @Input() set Type(extension: string) {
    if (extension) {
      extension = extension.split('.').pop();
      extension = extension.toLocaleLowerCase();
      if (extension === 'docx') { extension = 'doc'; }
      if (extension === 'xlsx') { extension = 'xls'; }
      if (extension === 'bak') { extension = 'sql'; }
      if (!this.avatarUrl) {
        this.avatarUrl = `${this.basePath}${extension}.png`;
        this.changeDetector.detectChanges();
      }
    }
  }

  errorLoadImage() { this.avatarUrl = `${this.basePath}notfound.png`; }

  constructor(private changeDetector: ChangeDetectorRef) { }
}
