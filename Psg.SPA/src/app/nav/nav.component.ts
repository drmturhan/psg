import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService, private uyarici: AlertifyService) { }

  ngOnInit() { }
  login() {
    this.authService.login(this.model).subscribe(data => {
      this.uyarici.success('Giriş başarılı');
    },
      error => { this.uyarici.warning('Giriş başarısız!')});
  }
  logout() {

    this.authService.userToken = null;
    localStorage.removeItem('token');
    this.uyarici.success('Çıkış yapıldı');
  }
  loggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

}
