import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
import { AuthUser } from '../_models/auth-user';
import { KisiFoto } from '../_models/foto';

@Injectable()
export class AuthService {
  baseUrl = environment.apiUrl;
  userToken: any;
  decodedToken: any;
  suankiKullanici: Kullanici;
  private fotoUrl = new BehaviorSubject<string>(environment.bosFotoUrl);
  suankiFotoUrl = this.fotoUrl.asObservable();
  constructor(private authHttp: HttpClient, private jwtHelperService: JwtHelperService, private router: Router) { }

  profilFotografiDegisti(url: string) {
    this.fotoUrl.next(url);
  }
  profilFotografiYap(fotoId: number) {
    return this.authHttp.post(`${this.baseUrl}profilim/profilFotografiYap`, fotoId);
  }

  login(model: any) {

    return this.authHttp.post<AuthUser>(this.baseUrl + 'account/girisyap', model,
      {
        headers: new HttpHeaders().set('Content-Type', 'application/json')
      }).map(gelenNesne => {
        localStorage.setItem('access_token', gelenNesne.tokenString);
        localStorage.setItem('kullanici', JSON.stringify(gelenNesne.kullanici));
        this.kullaniciAyarlariYap(gelenNesne.tokenString, gelenNesne.kullanici);
      });
  }
  public kullaniciAyarlariYap(token = null, kullanici = null) {

    if (token == null) {
      this.userToken = localStorage.getItem('access_token');
    } else {
      this.userToken = token;
    }
    if (kullanici == null) {
      this.suankiKullanici = JSON.parse(localStorage.getItem('kullanici'));
    } else {
      this.suankiKullanici = kullanici;
    }
    if (this.userToken === '' || this.suankiKullanici === null) {
      return;
    }
    let url = environment.bosFotoUrl;
    if (this.suankiKullanici.profilFotoUrl !== '') {
      url = this.suankiKullanici.profilFotoUrl;
    }
    this.profilFotografiDegisti(url);
  }

  kullaniciNumarasiniAl(): number {
    if (!this.suankiKullanici) {
      return -1;
    }
    return this.suankiKullanici.id;
  }
  kullaniciAktiveEt(kullaniciNo: number, kod: string) {
    return this.authHttp.get(`http://localhost:55126/api/account/kullaniciepostasinionayla?userId=${kullaniciNo}&code=${kod}`);
  }
  yenidenAktivasyonkoduGonder(kullaniciAdi: string, epostaAdresi: string) {
    return this.authHttp.
      get(`http://localhost:55126/api/account/onaykodunubirdahagonder/?kullaniciAdi=${kullaniciAdi}&eposta=${epostaAdresi}`);
  }

  kullaniciGuvenlikKoduDogrumu(kod: string) {
    return this.authHttp.get(`http://localhost:55126/api/account/guvenlikkodudogrumu?kod=${kod}`);
  }
  kullaniciAdiniAl(): string {
    if (!this.suankiKullanici) {
      return null;
    }
    return this.suankiKullanici.tamAdi;
  }
  register(uyeBilgisi: UyeBilgisi) {
    return this.authHttp.post('http://localhost:55126/api/account/uyelikbaslat', uyeBilgisi,
      {
        headers: new HttpHeaders().set('Content-Type', 'application/json')
      });
  }
  fotografSil(fotoId: number) {
    return this.authHttp.delete(`${this.baseUrl}profilim/${fotoId}`);
  }
  loggedIn(): boolean {
    const token = this.jwtHelperService.tokenGetter();
    if (!token) {
      return false;
    }
    const durum = !this.jwtHelperService.isTokenExpired(token);
    return durum;
  }


  logout() {
    this.userToken = null;
    this.decodedToken = null;
    this.suankiKullanici = null;
    localStorage.removeItem('access_token');
    localStorage.removeItem('kullanici');
    this.router.navigate(['/']);
  }
  hataYonetici(hata: any) {
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
