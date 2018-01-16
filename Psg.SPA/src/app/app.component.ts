import { environment } from './../environments/environment';
import { Kullanici } from './_models/kullanici';
import { AuthService } from './_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Polisomnografiler';


  constructor(private authService: AuthService) {
    
  }


  public ngOnInit(): void {
    const token = localStorage.getItem('access_token');
    const kullanici:Kullanici = JSON.parse(localStorage.getItem('kullanici'));
    if (kullanici) {
      this.authService.suankiKullanici = kullanici;
      let url = environment.bosFotoUrl;
      if (this.authService.suankiKullanici.profilFotoUrl !== '')
        url = this.authService.suankiKullanici.profilFotoUrl;
      this.authService.kullaniciFotografiniDegistir(kullanici.profilFotoUrl);
    }
  }
}
