import { Observable } from 'rxjs/Observable';
import { ErrorHandler, Inject, NgZone, Injectable, Injector } from '@angular/core';
import { Router } from "@angular/router";
import { AlertifyService } from './_services/alertify.service';

@Injectable()
export class MTAppErrorHandler implements ErrorHandler {
  constructor(private ngZone: NgZone, @Inject(AlertifyService) private uyarici: AlertifyService, private injector: Injector) { }
  handleError(error: any): void {
    if (error.headers) {

      const uygulamaHatasi = error.headers.get('Uygulama-Hatası');
      if (uygulamaHatasi) {
        this.uyarici.error(uygulamaHatasi);
      }
      else {
        const serverError = error.json();
        let modelStateErrors = '';
        if (serverError) {
          for (const key in serverError) {
            if (serverError[key]) {
              modelStateErrors += serverError[key] + '\n';
            }
          }
          if (modelStateErrors)
            this.uyarici.error(modelStateErrors);
        }
      }
    }
    else
      this.uyarici.error('Beklenmedik bir hata oluştu. Bu nedenle ana sayfaya dönüldü. Hata mesajı almaya devam ediyorsanız, lütfen sistem yöneticisine bilgi veriniz!')
    const router: Router = this.injector.get(Router);
    router.navigate(['/yuklemeHatasi']);
  }
}
