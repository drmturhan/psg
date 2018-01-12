import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { KullaniciDuzeltComponent } from '../../kullanicilar/kullanici-duzelt/kullanici-duzelt.component';

@Injectable()
export class KullanicidakiDegisikliklerKaybolsunmuGuard implements CanDeactivate<KullaniciDuzeltComponent>{

    constructor() { }
    canDeactivate(component: KullaniciDuzeltComponent) {
        if (component.duzenlemeFormu) {
            return confirm("Devam etmek istiyor musun? Kaydedilmemiş bilgiler kaybolacaktır!");
        }
        return true;
    }


}