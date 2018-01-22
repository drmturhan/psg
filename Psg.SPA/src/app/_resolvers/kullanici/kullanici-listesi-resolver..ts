import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { forkJoin } from "rxjs/observable/forkJoin";
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici, KullanicilarVeriSeti } from '../../_models/kullanici';


@Injectable()
export class KullaniciListesiResolver implements Resolve<KullanicilarVeriSeti> {

  constructor(
    private service: KullaniciService,
    private router: Router
  ) {

  }
  donecekVeriSeti: KullanicilarVeriSeti=new KullanicilarVeriSeti();
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<KullanicilarVeriSeti> {

    var veriKaynaklari = [
      this.service.listeGetirKullanicilarTumu()
    ];
    return forkJoin(veriKaynaklari).map(data => {
      this.donecekVeriSeti.kullaniciSonuc = data[0];
      if (this.donecekVeriSeti && this.donecekVeriSeti.kullaniciSonuc.basarili) {
        return this.donecekVeriSeti;
      }
      else {
        if (!this.donecekVeriSeti.kullaniciSonuc.basarili) {
          this.router.navigate(['/yuklemeHatasi',this.donecekVeriSeti.kullaniciSonuc.hatalar]);
        }
      }
      const mesajlar: string[] = ['Kullanıcı listesi yükelenirken bir hata oluştu!'];
      this.router.navigate(['/yuklemeHatasi', mesajlar]);
      return null;
    });
  }
}




