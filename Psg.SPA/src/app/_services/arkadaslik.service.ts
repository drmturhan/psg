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
    arkadasliklariGetir(sorgu?: ArkadaslikSorgusu): Observable<ListeSonuc<ArkadasliklarimListe>> {
        if (sorgu == null) {
            sorgu = new ArkadaslikSorgusu();
            sorgu.aramaCumlesi = '';
            sorgu.sayfa = 1;
            sorgu.sayfaBuyuklugu = 10;

        }
        let params: HttpParams = new HttpParams();
        if (sorgu.teklifEdenler != null) {
            params = params.append('teklifEdenler', sorgu.teklifEdenler.toString());
        }
        if (sorgu.teklifEdilenler != null) {
            params = params.append('teklifEdilenler', sorgu.teklifEdilenler.toString());
        }
        if (sorgu.cevaplananlar != null) {
            params = params.append('cevaplananlar', sorgu.cevaplananlar.toString());
        }
        if (sorgu.cevapBeklenenler != null) {
            params = params.append('cevapBeklenenler', sorgu.cevapBeklenenler.toString());
        }
        if (sorgu.kabulEdilenler != null) {
            params = params.append('kabulEdilenler', sorgu.kabulEdilenler.toString());
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
            params = params.append('siralamaCumlesi', sorgu.siralamaCumlesi.toString());
        }
        return this.httpClient.get<ListeSonuc<ArkadasliklarimListe>>(`${this.url}/arkadasliklarim`, { params });
    }
    arkadaslikteklifEt(isteyenId: number, cevaplayanId: number) {
        return this.httpClient.post(`${this.url}arkadasliklarim/${isteyenId}/teklif/${cevaplayanId}`, '');
    }
    arkadaslikTeklifiniIptalEt(isteyenId: number, cevaplayanId: number) {
        return this.httpClient.post(`${this.url}arkadasliklarim/${isteyenId}/teklifiptal/${cevaplayanId}`, '');
    }
    arkadaslikTeklifineKararVer(isteyenId: number, cevaplayanId: number, karar: boolean) {
        return this.httpClient.post(`${this.url}arkadasliklarim/${isteyenId}/kararver/${cevaplayanId}`,  karar);
    }
}