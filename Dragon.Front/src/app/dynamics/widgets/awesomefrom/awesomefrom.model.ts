export interface FromConfig {
  title?: string;
  submiturl?: string;
  fields?: [];
}

export interface FormFieldConfig {
  jsonProperty?: string;
  title?: string;
  validation?: any[];
}

export interface FormFieldValidation {
  type: string;
  message?: string;
}