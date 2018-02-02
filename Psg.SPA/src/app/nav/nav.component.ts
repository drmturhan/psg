import { Router } from '@angular/router';

import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  kullaniciAdi = '';
  fotoUrl: string;

  constructor(public authService: AuthService, private uyarici: AlertifyService, private router: Router) { }

  ngOnInit() {
    if (this.authService.suankiKullanici != null) {
      this.kullaniciAdi = this.authService.suankiKullanici.kullaniciAdi;
      this.authService.suankiFotoUrl.subscribe(fotoUrl => {
        this.fotoUrl = fotoUrl;
      });
    }
  }
  login() {
    this.authService.login(this.model).subscribe(data => {
      this.uyarici.success('Giriş başarılı');
      this.model = {};
      this.kullaniciAdi = this.authService.suankiKullanici.tamAdi;
    },
      error => this.uyarici.warning('Giriş başarısız!'),
      () => this.router.navigate(['/uyeler'])
    );
  }
  logout() {
    this.authService.logout();

  }


}
