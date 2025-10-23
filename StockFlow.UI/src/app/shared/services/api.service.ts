import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private http: HttpClient) {}

  get<T>(
    apiUrl: string,
    options?: { headers?: HttpHeaders; params?: HttpParams}
  ): Observable<T> {
    return this.http.get<T>(apiUrl, options);
  }
  post<T>(
    apiUrl: string,
    body?: any,
    options?: { headers?: HttpHeaders; params?: HttpParams}
  ): Observable<T> {
    return this.http.post<T>(apiUrl, body, options);
  }
}
