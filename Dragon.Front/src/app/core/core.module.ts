import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { IconModule } from './icon/icon.module';

@NgModule({
  declarations: [SidebarComponent, HeaderComponent],
  imports: [
    CommonModule,
    IconModule,
    RouterModule,
    TranslateModule,
    NgScrollbarModule.withConfig({
      visibility: 'hover',
      appearance: 'standard',
    }),
  ],
  exports: [TooltipModule, HeaderComponent, SidebarComponent],
})
export class CoreModule {}
