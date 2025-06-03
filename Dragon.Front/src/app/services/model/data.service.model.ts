export interface ApiResponse<T> {
  data: T;
  message: string;
  status: number;
}

export const StatusFlags = {
  Success: 1,
  Failed: 2,
  AlreadyExists: 3,
  DependencyExists: 4,
  NotPermitted: 5
};

export const eMessageType = {
  Error: 'error',
  Success: 'success',
  Info: 'info',
  Warning: 'warning',
  Question: 'question'
};

// export const eMessageIcon = {
//   Error: "error",
//   Success: 'success',
//   Info: 'info',
//   Warning: 'warning',
// };

