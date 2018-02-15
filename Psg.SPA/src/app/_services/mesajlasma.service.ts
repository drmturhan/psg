import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MesajlasmaSorgusu } from '../_models/sorgular/mesajlar-sorgusu';
import { AuthService } from './auth.service';
import { MesajListe, MesajYaratma } from '../_models/mesaj-liste';
import { ListeSonuc } from '../_models/sonuc';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { AppError } from '../_hatalar/app-error';

@Injectable()
export class MesajlasmaService {
    url = environment.apiUrl;

    constructor(private authService: AuthService, private httpClient: HttpClient) {

    }
    listeGetirMesajlar(kullaniciNo: number, sorgu?: MesajlasmaSorgusu): Observable<ListeSonuc<MesajListe>> {

        if (kullaniciNo !== this.authService.suankiKullanici.id) {
            return Observable.throw(new AppError('Güvenlik sorunu'));
        }
        if (sorgu == null) {
            sorgu = this.onTanimliSorgu(kullaniciNo);
        }
        let sorguParametreleri: HttpParams = this.sorguParametreleriniYarat(sorgu);
        if (sorgu.gelenMesajlar === true) {

            sorguParametreleri = sorguParametreleri.append('gelenMesajlar', 'true');
        }
        if (sorgu.gidenMesajlar === true) {

            sorguParametreleri = sorguParametreleri.append('gidenMesajlar', 'true');
        }
        if (sorgu.okunmamisMesajlar === true) {

            sorguParametreleri = sorguParametreleri.append('okunmamisMesajlar', 'true');
        }
        return this.httpClient.
            get<ListeSonuc<MesajListe>>(`${this.url}/kullanicilar/${kullaniciNo}/mesajlasmalar`, { params: sorguParametreleri });
    }
    mesajYiginiGetir(kullaniciNo: number, digerKullaniciNo: number, sorgu?: MesajlasmaSorgusu): Observable<ListeSonuc<MesajListe>> {
        if (kullaniciNo !== this.authService.suankiKullanici.id) {
            return Observable.throw(new AppError('Güvenlik sorunu'));
        }
        if (+digerKullaniciNo <= 0) {
            return Observable.throw(new AppError('Kullanıcı bilgisi yok!'));
        }
        if (sorgu == null) {
            sorgu = this.onTanimliSorgu(kullaniciNo);
        }
        sorgu.digerKullaniciNo = digerKullaniciNo;
        const sorguParametreleri: HttpParams = this.sorguParametreleriniYarat(sorgu);

        return this.httpClient.
            get<ListeSonuc<MesajListe>>(`${this.url}/kullanicilar/${kullaniciNo}/mesajlasmalar/yigin/${digerKullaniciNo}`,
                { params: sorguParametreleri });
    }
    mesajGonder(mesaj: MesajYaratma) {
        return this.httpClient.post(`${this.url}/kullanicilar/${mesaj.gonderenNo}/mesajlasmalar/`, mesaj);
    }
    mesajSil(id: number) {
        return this.httpClient.post(`${this.url}/kullanicilar/${this.authService.suankiKullanici.id}/mesajlasmalar/${id}`, {});
    }

    private onTanimliSorgu(id: number): MesajlasmaSorgusu {
        const sorgu = new MesajlasmaSorgusu();
        sorgu.aramaCumlesi = '';
        sorgu.sayfa = 1;
        sorgu.sayfaBuyuklugu = 10;
        sorgu.kullaniciNo = id;
        sorgu.okunmamisMesajlar = true;
        return sorgu;
    }
    private sorguParametreleriniYarat(sorgu: MesajlasmaSorgusu): HttpParams {
        if (sorgu === null) {
            throw new AppError('Mesajlaşma sorgusu boş olmamalı!');
        }

        let params: HttpParams = new HttpParams();
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
        if (sorgu.digerKullaniciNo != null) {
            params = params.append('digerKullaniciNo', sorgu.digerKullaniciNo.toString());
        }
        return params;
    }
    okunduOlarakIsaretle(kullaniciNo: number, mesajId: number) {
        return this.httpClient.post(`${this.url}/kullanicilar/${this.authService.suankiKullanici.id}/mesajlasmalar/${mesajId}/okundu`, {});
    }
}