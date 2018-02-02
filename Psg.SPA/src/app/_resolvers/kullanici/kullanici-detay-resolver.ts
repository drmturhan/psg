import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { forkJoin } from 'rxjs/observable/forkJoin';
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici } from '../../_models/kullanici';
import { KayitSonuc } from '../../_models/sonuc';


@Injectable()
export class KullaniciDetayResolver implements Resolve<KayitSonuc<Kullanici>> {

    constructor(
        private kullaniciService: KullaniciService,
        private router: Router
    ) {

    }
    donecekVeriSeti: KayitSonuc<Kullanici> = new KayitSonuc<Kullanici>();
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<KayitSonuc<Kullanici>> {
        const id = route.params['id'];
        if (isNaN(id)) {

            this.router.navigate(['/kullanicilar']);
            return null;
        }
        const veriKaynaklari = [
            this.kullaniciService.kullaniciBul(id)
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti = data[0];
            if (this.donecekVeriSeti && this.donecekVeriSeti.basarili) {
                return this.donecekVeriSeti;
            }
            this.router.navigate(['/kullanicilar']);
            return null;
        });
    }
}




