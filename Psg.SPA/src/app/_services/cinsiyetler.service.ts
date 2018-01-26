import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { ArkadaslikListe } from '../_models/arkadaslik-liste';
import { Observable } from 'rxjs/Observable';
import { Kullanici, KullaniciYaz } from '../_models/kullanici';


@Injectable()
export class CinsiyetlerService extends DataService {

    constructor( http: HttpClient) {
        super('cinsiyetler', http);
    }
}