import { AuthService } from './../_services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService, private router:Router,private uyarici: AlertifyService) {


  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (this.auth.loggedIn()) { return true; }
    this.uyarici.error('Bu sayfaya girebilmek için sisteme giriş yapmalısınız!');
    this.router.navigate(['anasayfa']);
    return false;
  }
}
