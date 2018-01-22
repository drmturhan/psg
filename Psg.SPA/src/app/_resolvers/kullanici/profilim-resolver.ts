import { Kullanici, KullanicilarVeriSeti, KullaniciVeriSeti } from './../../_models/kullanici';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { forkJoin } from "rxjs/observable/forkJoin";
import { KullaniciService } from './../../_services/kullanici.service';
import { KullaniciYaz } from '../../_models/kullanici';
import { AuthService } from '../../_services/auth.service';
import { Cinsiyet } from '../../_models/foto';




@Injectable()
export class ProfilimResolver implements Resolve<KullaniciVeriSeti> {

    constructor(
        private service: KullaniciService,
        private router: Router,
        private authService: AuthService
    ) {

    }
    donecekVeriSeti: KullaniciVeriSeti = new KullaniciVeriSeti();

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<KullaniciVeriSeti> {
        let id = this.authService.kullaniciNumarasiniAl();
        if (isNaN(id)) {

            const mesajlar: string[] = ['Aktif kullanıcı yok!', 'Profil bilgilerini görmek için önce giriş yapmalısınız!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
        }
        var veriKaynaklari = [
            this.service.kullaniciBulDegistirmekIcin(id),
            this.service.listeGetirCinsiyetler()
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti.kullaniciSonuc = data[0];
            this.donecekVeriSeti.cinsiyetler = data[1];
            if (this.donecekVeriSeti)
                return this.donecekVeriSeti;
            const mesajlar: string[] = ['Kullanıcı bilgisi yüklenemedi', 'Bu nedenle profil bilgilerinizi düzeltme ekranı açılmadı!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
            return null;
        });
    }
}




