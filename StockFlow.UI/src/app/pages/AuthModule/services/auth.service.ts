import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EndPoints } from '../../../shared/constants/end-points';
import { ApiService } from '../../../shared/services/api.service';
import { ApiResponse } from '../../../shared/models/api-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private apiService: ApiService) { }
  login(credentials: { email: string; password: string }): Observable<any>{
    const url = `${EndPoints.AUTH.LOGIN}`;
    return this.apiService.post<ApiResponse<boolean>>(url, credentials);
  }
  register(data: { userName: string; emailId: string; password: string }): Observable<any>{
    const url = `${EndPoints.AUTH.REGISTER}`;
    return this.apiService.post<ApiResponse<boolean>>(url, data);
  }
}
