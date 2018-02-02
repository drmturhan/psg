import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpSentEvent,
    HttpHeaderResponse,
    HttpProgressEvent,
    HttpResponse,
    HttpUserEvent,
    HttpErrorResponse,
    HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { AppError } from '../_hatalar/app-error';
import { BadInputError } from '../_hatalar/bad-input';



Injectable();


export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler):
        Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
        return next.handle(req).catch(hata => {
            if (hata instanceof HttpErrorResponse) {
                const appError = hata.headers.get('Application-Error');
                if (appError) {
                    return Observable.throw(new AppError(appError));
                }
            }
            if (hata.error && hata.error['hatalar']) {
                console.log(hata.error);
                return Observable.throw(hata.error.hatalar[0]);
            }
            const serverError = hata.error;
            let modelStateErrors = '';
            if (serverError && typeof serverError === 'object') {
                for (const key in serverError) {
                    if (serverError[key]) {
                        modelStateErrors += serverError[key] + '\n';
                    }
                }
            }
            return Observable.throw(modelStateErrors || serverError || 'Server error');
        });
    }
}
export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};






