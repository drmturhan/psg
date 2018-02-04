import { ArkadasliklarimListe } from './../../_models/arkadaslik-liste';
import { Kullanici, KullanicilarVeriSeti } from './../../_models/kullanici';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { forkJoin } from 'rxjs/observable/forkJoin';
import { KullaniciService } from './../../_services/kullanici.service';
import { AuthService } from '../../_services/auth.service';
import { Cinsiyet } from '../../_models/foto';
import { ListeSonuc } from '../../_models/sonuc';
import { ArkadaslikService } from '../../_services/arkadaslik.service';
import { HttpParams } from '@angular/common/http';
import { ArkadaslikSorgusu } from '../../_models/sorgular/arkadaslik-sorgusu';




@Injectable()
export class ArkadaslarimResolver implements Resolve<ListeSonuc<ArkadasliklarimListe>> {

    constructor(
        private service: KullaniciService,
        private router: Router,
        private authService: AuthService,
        private arkadaslikService: ArkadaslikService
    ) {

    }
    donecekVeriSeti: ListeSonuc<ArkadasliklarimListe>;

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ListeSonuc<ArkadasliklarimListe>> {
        const id = this.authService.suankiKullanici.id;
        if (isNaN(id)) {

            const mesajlar: string[] = ['Aktif kullanıcı yok!', 'Arkadaşlarını görmek için önce giriş yapmalısınız!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
        }
        const sorgu = new ArkadaslikSorgusu();
        sorgu.sayfa = 1;
        sorgu.teklifEdenKullaniciNo = this.authService.suankiKullanici.id;

        const veriKaynaklari = [
            this.arkadaslikService.arkadasliklariGetir(sorgu)
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti = data[0];
            if (this.donecekVeriSeti) {
                return this.donecekVeriSeti;
            }
            const mesajlar: string[] = ['Arkadaşlık bilgileri yüklenemedi!', 'Bu nedenle arkadaşlarım sayfası açılamadı!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
            return null;
        });
    }
}




