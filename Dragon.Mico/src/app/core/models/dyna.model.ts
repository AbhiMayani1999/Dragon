export interface OptionTransfer {
  table: string;
  key: string;
  value: string;
  keystore: string;
  options: KeyValuePair[];
}

export interface KeyValuePair {
  key?: any;
  value?: any;
}
