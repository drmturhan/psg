import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { forkJoin } from "rxjs/observable/forkJoin";
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici } from '../../_models/kullanici';


@Injectable()
export class KullaniciDetayResolver implements Resolve<Kullanici> {

    constructor(
        private service: KullaniciService,
        private router: Router
    ) {

    }
    donecekVeriSeti: Kullanici=new Kullanici();;
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Kullanici> {
        let id = route.params['id'];
        if (isNaN(id)) {
            
            this.router.navigate(['/kullanicilar']);
            return null;
        }
        var veriKaynaklari = [
            this.service.kullaniciBul(id)
        ];
        return forkJoin(veriKaynaklari).map(data => {
            this.donecekVeriSeti = data[0];
            if (this.donecekVeriSeti) {
                return this.donecekVeriSeti;
            }
            this.router.navigate(['/kullanicilar']);
            return null;
        });
    }
}




