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
  kullaniciAdi:string='Hoşgeldiniz';
  
   
  constructor(private authService: AuthService, private uyarici: AlertifyService,private router:Router) { }

  ngOnInit() { 
    this.kullaniciAdi=this.authService.kullaniciAdiniAl();
  }
  login() {
    this.authService.login(this.model).subscribe(data => {
      this.uyarici.success('Giriş başarılı');
      this.model={};
      this.kullaniciAdi=this.authService.kullaniciAdiniAl();
    },
      error => this.uyarici.warning('Giriş başarısız!'),
      ()=>this.router.navigate(['/uyeler'])
    );
  }
  logout() {

    this.authService.userToken = null;
    localStorage.removeItem('access_token');
    this.uyarici.warning('Çıkış yapıldı');
    ()=>this.router.navigate(['/anasayfa'])
  }
  loggedIn(): boolean {
    return this.authService.loggedIn();

  }

}
