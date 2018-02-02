
import {
    HttpInterceptor,
    HttpEvent,
    HttpHandler,
    HttpRequest,
    HttpHeaders,
    HttpResponse,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Inject } from '@angular/core';

export class AuthInterceptor implements HttpInterceptor {

    constructor( @Inject(JwtHelperService) private helper: JwtHelperService) {
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem('access_token');
        const tokenGecerli = !this.helper.isTokenExpired(token);
        if (tokenGecerli === true) {
            const tokenEklenenReq = req.clone(
                { setHeaders: { Authorization: 'Bearer ' + token } });
            return next.handle(tokenEklenenReq).do((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    // do stuff with response if you want
                }
            }, (err: any) => {
                if (err instanceof HttpErrorResponse){
                    if (err.status === 401) {
                        // redirect to the login route
                        // or show a modal
                    }
                }
            });
        }
        return next.handle(req);
    }
}

