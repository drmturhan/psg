import { Observable } from 'rxjs/Observable';
import { ErrorHandler, Inject, NgZone, Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';


@Injectable()
export class MTAppErrorHandler implements ErrorHandler {
    constructor(private uyarici: AlertifyService) { }
    handleError(error: any): void {
        console.log(error);
    }
}
