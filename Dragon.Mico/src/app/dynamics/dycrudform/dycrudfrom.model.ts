import { KeyValuePair, OptionTransfer } from "@core/models/dyna.model";

export interface FromConfig {
  title?: string;
  submiturl?: string;
  fields?: any[];

  parentIdentifier?: number;
  data?: any;
}

export interface FormFieldConfig {
  jsonProperty?: string;
  title?: string;
  validation?: FormFieldValidation[];
  checked?: boolean;
  options?: KeyValuePair[];
  optionTransfer?: OptionTransfer;

  identifier: boolean;
}

export interface FormFieldValidation {
  type?: string;
  message?: string;
}

export const FormFields = { Id: 'id' };
