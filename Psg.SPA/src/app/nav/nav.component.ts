import { Router } from '@angular/router';

import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { InternetBaglantisiError } from '../_hatalar/bad-input';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  sifreKurtarModel: any = {};
  sifreKurtarmaGorunsun = false;
  girisFormuGorunsun = false;

  constructor(public authService: AuthService, private uyarici: AlertifyService, private router: Router) { }

  ngOnInit() {

  }
  login() {
    this.authService.login(this.model).subscribe(data => {
      this.uyarici.success('Giriş başarılı.');
      this.model = {};
      this.authService.hataliGirisSayisi = 0;
      this.girisFormuGorunsun = false;
    },
      error => {
        this.authService.hataliGirisSayisi++;
        throw error;
      }
    );
  }
  logout() {
    this.authService.logout().subscribe(() => {
      this.uyarici.success('Çıkış yapıldı.');
      this.girisFormuGorunsun = true;
    },
      error => {
        this.uyarici.warning('Çıkış işlemleri tamamlanmadı. Lütfen tekrar deneyiniz.');
        this.uyarici.warning('Tam çıkış yapmazsanız güvenlik açığı oluşabilir!');
        if (error instanceof InternetBaglantisiError) {
          throw error;
        }
      });
  }
  sifremiUnuttumEkraniAc() {
    this.sifreKurtarmaGorunsun = true;
  }
  sifreKurtar() {

  };
}
