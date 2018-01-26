import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { forkJoin } from 'rxjs/observable/forkJoin';
import { KullaniciService } from './../../_services/kullanici.service';
import { CinsiyetlerService } from '../../_services/cinsiyetler.service';
import { Kullanici, KullanicilarVeriSeti, KullaniciVeriSeti } from './../../_models/kullanici';
import { KullaniciYaz } from '../../_models/kullanici';
import { AuthService } from '../../_services/auth.service';
import { Cinsiyet } from '../../_models/foto';
import { KayitSonuc } from '../../_models/sonuc';

@Injectable()
export class ProfilimResolver implements Resolve<KullaniciVeriSeti> {

    constructor(
        private kullaniciService: KullaniciService,
        private cinsiyetlerService: CinsiyetlerService,
        private router: Router,
        private authService: AuthService
    ) {}

    donecekVeriSeti: KullaniciVeriSeti = new KullaniciVeriSeti();
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<KullaniciVeriSeti> {
        const id = this.authService.kullaniciNumarasiniAl();
        if (isNaN(id)) {

            const mesajlar: string[] = ['Aktif kullanıcı yok!', 'Profil bilgilerini görmek için önce giriş yapmalısınız!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
        }
        const veriKaynaklari = [
            this.kullaniciService.kullaniciBulDegistirmekIcin(id),
            this.cinsiyetlerService.list<KayitSonuc<Kullanici>>()
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti.kullaniciSonuc = data[0];
            this.donecekVeriSeti.cinsiyetler = data[1];
            if (this.donecekVeriSeti) {
                return this.donecekVeriSeti;
            }
            const mesajlar: string[] = ['Kullanıcı bilgisi yüklenemedi', 'Bu nedenle profil bilgilerinizi düzeltme ekranı açılmadı!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
            return null;
        });
    }
}




