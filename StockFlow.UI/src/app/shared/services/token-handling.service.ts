import { ApiService } from './api.service';
import { inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';
import { SystemConstants } from '../constants/system-constants';
import { Observable, of } from 'rxjs';
import { ApiResponse } from '../models/api-response';
import { EndPoints } from '../constants/end-points';

interface ValidateRefreshResponse {
  refreshed: boolean;
  message?: string;
}
@Injectable({
  providedIn: 'root'
})
export class TokenHandlingService {

  private jwtHelper = new JwtHelperService();
  private cookieService = inject(CookieService);
  private apiService = inject(ApiService);

  getAccessToken() {
    return this.cookieService.get(SystemConstants.TOKENS.ACCESS_TOKEN);
  }
  decodeToken(token: string): any {
    try {
      return this.jwtHelper.decodeToken(token);
    } catch {
      return null;
    }
  }
  isTokenExpired(token: string): boolean {
    try {
      return this.jwtHelper.isTokenExpired(token);
    } catch {
      return true;
    }
  }
  clearAuthCookies(): void {
    this.cookieService.delete(SystemConstants.TOKENS.ACCESS_TOKEN, '/');
    this.cookieService.delete(SystemConstants.TOKENS.REFRESH_TOKEN, '/');
  }
  validateToken(): Observable<{
    isValid: boolean;
    isExpired: boolean | null;
  }> {
    const accessToken = this.getAccessToken();

    if (!accessToken) {
      return of({ isValid: false, isExpired: null});
    }

    const isExpired = this.isTokenExpired(accessToken);
    const decodedAccess = this.decodeToken(accessToken);

    if (!decodedAccess) {
      return of({ isValid: false, isExpired: null});
    }

    return of({
      isValid: !isExpired,
      isExpired
    });
  }
  validateRefreshToken(): Observable<ApiResponse<ValidateRefreshResponse>> {
    const url = `${EndPoints.AUTH.VALIDATE_REFRESH}`;
    return this.apiService.get<ApiResponse<ValidateRefreshResponse>>(url);
  }
}
