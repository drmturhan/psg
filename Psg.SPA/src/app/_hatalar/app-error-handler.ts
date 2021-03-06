import { ErrorHandler, Inject, Injectable, Injector, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { AlertifyService } from '../_services/alertify.service';
import { BadInputError, InternetBaglantisiError } from './bad-input';

@Injectable()
export class MTAppErrorHandler implements ErrorHandler {
    constructor(private uyarici: AlertifyService) { }
    handleError(error: any): void {
        if (error instanceof InternetBaglantisiError) {
            this.uyarici.error((<InternetBaglantisiError>error).orjinalHata);
        } else if (error instanceof BadInputError) {
            this.uyarici.error((<BadInputError>error).orjinalHata);
        } else {
            if (error['error']) {
                this.uyarici.error(error.error);
            } else {
                console.log(error);
            }
        }
    }
}

export const AppErrorProvider = {
    provide: ErrorHandler,
    useClass: MTAppErrorHandler
};