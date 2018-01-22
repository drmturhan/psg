import { ArkadaslikListe } from './../../_models/arkadaslik-liste';
import { Kullanici, KullanicilarVeriSeti } from './../../_models/kullanici';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { forkJoin } from "rxjs/observable/forkJoin";
import { KullaniciService } from './../../_services/kullanici.service';
import { AuthService } from '../../_services/auth.service';
import { Cinsiyet } from '../../_models/foto';
import { ListeSonuc } from '../../_models/sonuc';




@Injectable()
export class ArkadaslarimResolver implements Resolve<ListeSonuc<ArkadaslikListe>> {

    constructor(
        private service: KullaniciService,
        private router: Router,
        private authService: AuthService
    ) {

    }
    donecekVeriSeti: ListeSonuc<ArkadaslikListe>;

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ListeSonuc<ArkadaslikListe>> {
        let id = this.authService.kullaniciNumarasiniAl();
        if (isNaN(id)) {

            const mesajlar: string[] = ['Aktif kullanıcı yok!', 'Arkadaşlarını görmek için önce giriş yapmalısınız!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
        }
        var veriKaynaklari = [
            this.service.arkadasliklariGetir()
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti= data[0];
            if (this.donecekVeriSeti)
                return this.donecekVeriSeti;
            const mesajlar: string[] = ['Arkadaşlık bilgileri yüklenemedi!', 'Bu nedenle arkadaşlarım sayfası açılamadı!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
            return null;
        });
    }
}




