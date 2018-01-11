import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { HttpClient } from '@angular/common/http';

import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  baseUrl = 'http://localhost:55126/api/auth/';
  userToken: any;

  constructor(private authHttp: HttpClient, private helper: JwtHelperService) { }
  login(model: any) {

    return this.authHttp.post(this.baseUrl + 'girisyap', model).map((response: Response) => {
      const token = response['tokenString'];
      if (token) {
        localStorage.setItem('access_token', token);
        this.userToken = token;
        console.log(this.helper.decodeToken(token));
      }
    });
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
