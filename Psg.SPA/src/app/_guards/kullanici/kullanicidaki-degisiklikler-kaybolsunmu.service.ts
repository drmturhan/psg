import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

import { ProfilimComponent } from '../../uyelik/profilim/profilim.component';

@Injectable()
export class KullanicidakiDegisikliklerKaybolsunmuGuard implements CanDeactivate<ProfilimComponent>{

    constructor() { }
    canDeactivate(component: ProfilimComponent) {
        if (component.duzenlemeFormu) {
            return confirm('Devam etmek istiyor musun? Kaydedilmemiş bilgiler kaybolacaktır!');
        }
        return true;
    }


}