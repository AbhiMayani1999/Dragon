import { Directive, Input, ViewContainerRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FromfieldMapper } from './awesomeform.mapper';
import { FormFieldConfig } from './awesomefrom.model';

@Directive({ selector: '[awformfield]' })
export class AwesomeformfieldDirective {
  componentRef: any;
  constructor(private container: ViewContainerRef) { }
  @Input() set config(data: AwFromFieldInput) {
    const componentType = Object.keys(data.field)[0];
    const widget = FromfieldMapper[componentType];
    if (widget) {
      if(!this.componentRef){
        this.componentRef = this.container.createComponent(widget);
      }
      this.componentRef.instance.config = data.field[componentType];
      this.componentRef.instance.formGroup = data.formGroup;
      this.componentRef.instance.isSubmitted = data.isSubmitted;
    } else {
      console.log(`Form Component type not found: ${componentType}`);
    }
  }
}

export interface AwFromFieldInput {
  field: FormFieldConfig;
  formGroup: FormGroup;
  isSubmitted: boolean;
}
