import { environment } from './../environments/environment';
import { Kullanici } from './_models/kullanici';
import { AuthService } from './_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private _localeService: BsLocaleService,
    private jwtHelperService: JwtHelperService) {
  }

  public ngOnInit(): void {
    this._localeService.use('tr');
    const token = localStorage.getItem('access_token');
    const kullanici: Kullanici = JSON.parse(localStorage.getItem('kullanici'));
    if (token) {
      this.authService.decodedToken = this.jwtHelperService.decodeToken(token);
    }
    if (kullanici) {
      this.authService.suankiKullanici = kullanici;
      let url = environment.bosFotoUrl;
      if (this.authService.suankiKullanici.profilFotoUrl != null && this.authService.suankiKullanici.profilFotoUrl !== '') {
        url = this.authService.suankiKullanici.profilFotoUrl;
      }
      this.authService.kullaniciFotografiniDegistir(url);
    }
  }
}
