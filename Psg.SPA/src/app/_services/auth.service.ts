import { Kullanici } from './../_models/kullanici';
import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  baseUrl = 'http://localhost:55126/api/auth/';
  userToken: any;
  suankiKullanici: Kullanici
  private fotoUrl = new BehaviorSubject<string>("../../assets/images/user.png");
  suankiFotoUrl = this.fotoUrl.asObservable();
  constructor(private authHttp: HttpClient, private helper: JwtHelperService) { }

  kullaniciFotografiniDegistir(fotoUrl: string) {
    this.fotoUrl.next(fotoUrl);
  }

  login(model: any) {

    return this.authHttp.post(this.baseUrl + 'girisyap', model).map((response: Response) => {
      const token = response['tokenString'];
      const kullanici = response["kullanici"];
      if (token) {
        localStorage.setItem('access_token', token);
        localStorage.setItem('kullanici', JSON.stringify(kullanici));
        this.userToken = token;
        this.suankiKullanici = kullanici;
        this.kullaniciFotografiniDegistir(this.suankiKullanici.asilFotoUrl);
      }
    });
  }
  kullaniciNumarasiniAl(): number {
    const token = localStorage.getItem('access_token');
    if (token) {
      console.log(this.helper.getTokenExpirationDate(token));
      return +this.helper.decodeToken(token)['nameid'];
    }
  }
  kullaniciAdiniAl(): string {
    const token = localStorage.getItem('access_token');
    if (token) {
      console.log(this.helper.getTokenExpirationDate(token));
      return this.helper.decodeToken(token)['unique_name'];
    }
  }
  register(model: any) {
    return this.authHttp.post(this.baseUrl + 'uyeol', model);
  }
  loggedIn(): boolean {
    return localStorage.getItem('access_token') != null;
  }
  hataYoneti(hata: any) {
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
