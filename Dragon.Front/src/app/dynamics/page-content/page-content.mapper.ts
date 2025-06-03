import { AwesomedatatableComponent } from '../widgets/awesomedatatable/awesomedatatable.component';
import { GeneratorComponent } from '../widgets/generator/generator.component';

export enum WidgetNames {
  Generator = 'generator',
  Table = 'table',
}

export const PageContentMapper = {
  [WidgetNames.Generator]: GeneratorComponent,
  [WidgetNames.Table]: AwesomedatatableComponent,
};
