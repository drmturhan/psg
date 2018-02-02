import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { ArkadaslikListe } from '../_models/arkadaslik-liste';
import { Observable } from 'rxjs/Observable';
import { Kullanici, KullaniciYaz } from '../_models/kullanici';
import { environment } from '../../environments/environment';
import { Cinsiyet } from '../_models/foto';


@Injectable()
export class CinsiyetlerService {

    constructor(private dataService: DataService) {
        this.url = environment.apiUrl;
    }
    private url: string;

    list() {
        return this.dataService.get<Cinsiyet[]>(`${this.url}/cinsiyetler`);
    }
}