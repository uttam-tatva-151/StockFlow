export interface ApiResponse<T = any> {
  succeeded: boolean;
  message?: string;
  data?: T | null;
  errors?: any;
}