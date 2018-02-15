import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { forkJoin } from 'rxjs/observable/forkJoin';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici, KullanicilarVeriSeti } from '../../_models/kullanici';
import { ListeSonuc } from '../../_models/sonuc';
import { KullaniciSorgusu } from '../../_models/sorgular/kullanici-sorgusu';



@Injectable()
export class KullaniciListesiResolver implements Resolve<KullanicilarVeriSeti> {

  constructor(
    private kullaniciService: KullaniciService,
    private router: Router
  ) { }
  donecekVeriSeti: KullanicilarVeriSeti = new KullanicilarVeriSeti();
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<KullanicilarVeriSeti> {
    const veriKaynaklari = [
      this.kullaniciService.listeGetirKullanicilar()
    ];
    return forkJoin(veriKaynaklari).map(data => {
      this.donecekVeriSeti.kullaniciSonuc = data[0];
      if (this.donecekVeriSeti && this.donecekVeriSeti.kullaniciSonuc.basarili) {
        return this.donecekVeriSeti;
      } else {
        if (!this.donecekVeriSeti.kullaniciSonuc.basarili) {
          this.router.navigate(['/yuklemeHatasi', this.donecekVeriSeti.kullaniciSonuc.hatalar]);
        }
      }
      const mesajlar: string[] = ['Kullanıcı listesi yükelenirken bir hata oluştu!'];
      this.router.navigateByUrl('/yuklemeHatasi');
      return null;
    });
  }
}




