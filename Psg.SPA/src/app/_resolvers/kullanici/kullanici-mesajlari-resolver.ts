import { MesajListe } from '../../_models/mesaj-liste';
import { ListeSonuc } from '../../_models/sonuc';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { MesajlasmaService } from '../../_services/mesajlasma.service';
import { AuthService } from '../../_services/auth.service';
import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { forkJoin } from 'rxjs/observable/forkJoin';

@Injectable()
export class KullaniciMesajlariResolver implements Resolve<ListeSonuc<MesajListe>> {

    constructor(
        private mesajlarService: MesajlasmaService,
        private router: Router,
        private authService: AuthService
    ) {

    }
    donecekVeriSeti: ListeSonuc<MesajListe>;

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ListeSonuc<MesajListe>> {
        const id = this.authService.suankiKullanici.id;
        if (isNaN(id)) {

            const mesajlar: string[] = ['Aktif kullanıcı yok!', 'Arkadaşlarını görmek için önce giriş yapmalısınız!'];
            this.router.navigate(['/yuklemeHatasi', mesajlar]);
        }
        const kullaniciNo = this.authService.suankiKullanici.id;
        const veriKaynaklari = [
            this.mesajlarService.listeGetirMesajlar(kullaniciNo)
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