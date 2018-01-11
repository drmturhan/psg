import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/operator/map';
import { Kullanici } from './../_models/kullanici';
import { environment } from './../../environments/environment';
import { HttpRequest } from '@angular/common/http/src/request';


@Injectable()
export class KullaniciService {
    baseUrl = environment.apiUrl;
    constructor(private http: Http) { }
    listeGetirKullanicilarTumu(): Observable<Kullanici[]> {
        return this.http
            .get(this.baseUrl + 'kullanicilar', this.jwt())
            .map(
            response => {
                console.log(response.json());
                return <Kullanici[]>response.json();
            },
            hatalar => this.hatalariYonet(hatalar));
    }
    kullaniciBul(id: number): Observable<Kullanici> {
        return this.http.get(this.baseUrl + `kullanicilar/${id}`, this.jwt())
            .map(
            response => {
                console.log(response);
                return <Kullanici>response.json();
            },
            hatalar => this.hatalariYonet(hatalar)
            );
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