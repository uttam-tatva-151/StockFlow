import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _http: HttpClient) { }
  login(credentials: { email: string; password: string }): Observable<any>{
    return this._http.post('/api/auth/login', credentials);
  }
  register(data: { userName: string; email: string; password: string }): Observable<any>{
    return this._http.post('/api/auth/register', data);
  }
}
