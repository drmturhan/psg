import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ListeSonuc } from '../_models/sonuc';
import { ArkadasliklarimListe } from '../_models/arkadaslik-liste';
import { environment } from '../../environments/environment';
import { ArkadaslikSorgusu } from '../_models/sorgular/arkadaslik-sorgusu';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class ArkadaslikService {

    url = environment.apiUrl;
    constructor(private httpClient: HttpClient) { }
    arkadasliklariGetir(sorgu: ArkadaslikSorgusu): Observable<ListeSonuc<ArkadasliklarimListe>> {
        let params: HttpParams = new HttpParams();
        if (sorgu.teklifEdenKullaniciNo != null) {
            params = params.append('teklifEdenKullaniciNo', sorgu.teklifEdenKullaniciNo.toString());
        }
        if (sorgu.cevapVerecekKullaniciNo != null) {
            params = params.append('cevapVerecekKullaniciNo', sorgu.cevapVerecekKullaniciNo.toString());
        }
        if (sorgu.sayfa != null) {
            params = params.append('sayfa', sorgu.sayfa.toString());
        }
        if (sorgu.aramaCumlesi != null) {
            params = params.append('aramaCumlesi', sorgu.aramaCumlesi.toString());
        }
        if (sorgu.sayfaBuyuklugu != null) {
            params = params.append('sayfaBuyuklugu', sorgu.sayfaBuyuklugu.toString());
        }
        if (sorgu.siralamaCumlesi != null) {
            params = params.append('sayfaBuyuklugu', sorgu.sayfaBuyuklugu.toString());
        }
        return this.httpClient.get<ListeSonuc<ArkadasliklarimListe>>(`${this.url}/arkadasliklar`, { params });
    }
}