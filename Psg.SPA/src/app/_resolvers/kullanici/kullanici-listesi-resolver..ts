import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { forkJoin } from "rxjs/observable/forkJoin";
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici } from '../../_models/kullanici';


@Injectable()
export class KullaniciListesiResolver implements Resolve<Kullanici[]> {

    constructor(
        private service: KullaniciService,
        private router: Router
    ) {

    }
    donecekVeriSeti: Kullanici[]=[];
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Kullanici[]> {
        
        var veriKaynaklari = [
            this.service.listeGetirKullanicilarTumu()
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti = data[0];
            if (this.donecekVeriSeti) {
                return this.donecekVeriSeti;
            }
            const mesajlar:string[]=['Kullanıcı listesi yükelenirken bir hata oluştu!'];
            this.router.navigate(['/yuklemeHatasi',mesajlar]);
            return null;
        });
    }
}




