import { Directive, Input, ViewContainerRef } from '@angular/core';
import { PageContentMapper } from './page-content.mapper';

@Directive({ selector: 'page-content' })
export class PageContentDirective {
  constructor(private container: ViewContainerRef) { }

  @Input() set widget(data: any) {
    const componentType = Object.keys(data)[0];
    const widget = PageContentMapper[componentType];
    if (widget) {
      const component: any = this.container.createComponent(widget);
      component.instance.config = data[componentType];
    } else {
      console.log(`Component type not found: ${componentType}`);
    }
  }
}
