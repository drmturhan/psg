import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import 'rxjs/operator/map';
import { Kullanici } from './../_models/kullanici';
import { environment } from './../../environments/environment';
import { HttpRequest } from '@angular/common/http/src/request';
import { KullaniciYaz } from '../_models/kullanici';
import { Cinsiyet } from '../_models/foto';


@Injectable()
export class KullaniciService {
    baseUrl = environment.apiUrl;
    constructor(private http: Http) { }
    listeGetirKullanicilarTumu(): Observable<Kullanici[]> {
        return this.http
            .get(this.baseUrl + 'kullanicilar', this.jwt())
            .map(
            response => {
                // console.log(response.json());
                return <Kullanici[]>response.json();
            },
            hatalar => this.hatalariYonet(hatalar));
    }
    kullaniciBul(id: number): Observable<Kullanici> {
        return this.http.get(this.baseUrl + `kullanicilar/${id}`, this.jwt())
            .map(
            response => {
                // console.log(response);
                return <Kullanici>response.json();
            },
            hatalar => this.hatalariYonet(hatalar)
            );
    }
    kullaniciBulDegistirmekIcin(id: number): Observable<KullaniciYaz> {
        return this.http.get(this.baseUrl + `kullanicilar/${id}?neden=yaz`, this.jwt())
            .map(
            response => {
                // console.log(response);
                return <KullaniciYaz>response.json();
            },
            hatalar => this.hatalariYonet(hatalar)
            );
    }

    listeGetirCinsiyetler(): Observable<Cinsiyet[]> {
        return this.http.get(this.baseUrl + 'cinsiyetler', this.jwt()).map(sonuc => {
            return <Cinsiyet[]>sonuc.json();
        },
            hatalar => this.hatalariYonet(hatalar));

    }
    guncelle(id: number, kullanici: Kullanici) {
        return this.http.put(this.baseUrl + 'kullanicilar/' + id, kullanici, this.jwt());
    }
    asilFotoYap(kullaniciNo: number, fotoId: number) {
        return this.http.post(this.baseUrl + 'kullanicilar/' + kullaniciNo + '/fotograflari/' + fotoId + '/asilYap', {}, this.jwt());

    }
    sil(kullaniciNo: number, fotoId: number) {
        return this.http.delete(this.baseUrl + 'kullanicilar/' + kullaniciNo + '/fotograflari/' + fotoId, this.jwt());
    }

    private jwt() {
        let token = localStorage.getItem('access_token');
        if (token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + token });
            headers.append('Content-type', 'application/json');
            return new RequestOptions({ headers: headers });
        }

    }
    hatalariYonet(hata: any) {
        const uygulamaHatasi = hata.headers.get('Uygulama-Hatasi');
        if (uygulamaHatasi) {
            return Observable.throw(uygulamaHatasi);
        }
        const serverHatalari = hata.json();
        let modelDurumHatalari = '';
        if (serverHatalari) {
            for (const key in serverHatalari) {
                if (serverHatalari[key]) {
                    modelDurumHatalari += serverHatalari[key] + '\n';
                }
            }
        }
    }
}