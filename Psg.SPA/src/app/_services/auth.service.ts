import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/switchMap';
import { Kullanici, UyeBilgisi } from './../_models/kullanici';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { BadInputError } from '../_hatalar/bad-input';
import { AppError } from '../_hatalar/app-error';

@Injectable()
export class AuthService {
  baseUrl = environment.apiUrl;
  userToken: any;
  suankiKullanici: Kullanici;
  private fotoUrl = new BehaviorSubject<string>(environment.bosFotoUrl);
  suankiFotoUrl = this.fotoUrl.asObservable();
  constructor(private authHttp: HttpClient, private helper: JwtHelperService, private router: Router) { }

  kullaniciFotografiniDegistir(fotoUrl: string) {
    this.fotoUrl.next(fotoUrl);
  }

  login(model: any) {

    return this.authHttp.post(this.baseUrl + 'auth/girisyap', model).map((response: Response) => {
      const token = response['tokenString'];
      const kullanici = response['kullanici'];
      if (token) {
        localStorage.setItem('access_token', token);
        localStorage.setItem('kullanici', JSON.stringify(kullanici));
        this.userToken = token;
        this.suankiKullanici = kullanici;
        let url = environment.bosFotoUrl;
        if (this.suankiKullanici.profilFotoUrl !== '') {
          url = this.suankiKullanici.profilFotoUrl;
        }
        this.kullaniciFotografiniDegistir(url);
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
  register(uyeBilgisi: UyeBilgisi) {
    return this.authHttp.post(this.baseUrl + 'auth/uyeol', uyeBilgisi)
      .catch((hata: Response) => {
        if (hata.status = 400) {
          return Observable.throw(new BadInputError(hata.json));
        }
        return Observable.throw(new AppError(hata.json()));
      });
  }
  loggedIn(): boolean {
    const token = localStorage.getItem('access_token');
    if (token) {
      if (!this.helper.isTokenExpired(token)) {
        return true;
      }
    }
    return false;
  }


  logout() {
    this.userToken = null;
    this.suankiKullanici = null;
    localStorage.removeItem('access_token');
    localStorage.removeItem('kullanici');
    this.router.navigate(['/']);
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
