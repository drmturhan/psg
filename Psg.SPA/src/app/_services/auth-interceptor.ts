
import {
    HttpInterceptor,
    HttpEvent,
    HttpHandler,
    HttpRequest,
    HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Inject } from '@angular/core';

export class AuthInterceptor implements HttpInterceptor {

    constructor( @Inject(JwtHelperService) private helper: JwtHelperService) {
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const istek = req.clone({ headers: this.baslikYarat() });
        return next.handle(istek);
    }

    private baslikYarat(): HttpHeaders {
        const token = localStorage.getItem('access_token');
        let headers: HttpHeaders;
        if (token) {
            const tokenGecerli = !this.helper.isTokenExpired(token);
            if (tokenGecerli) {
                headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
                headers.append('Content-type', 'application/json');
            }
        }
        return headers;
    }
}