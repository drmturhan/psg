import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataService {
    constructor(private http: HttpClient, private authService: AuthService) { }

    get<T>(url: string): Observable<T> {
        return this.http.get<T>(url);
    }

    post<T>(url: string, body: string): Observable<T> {
        return this.http.post<T>(url, body);
    }
    kaydet<T>(url: string, record: T): Observable<T> {
        return this.http.put<T>(url, record);
    }

    put<T>(url: string, body: string): Observable<T> {
        return this.http.put<T>(url, body);
    }

    delete<T>(url: string): Observable<T> {
        return this.http.delete<T>(url);
    }

    patch<T>(url: string, body: string): Observable<T> {
        return this.http.patch<T>(url, body);
    }
}