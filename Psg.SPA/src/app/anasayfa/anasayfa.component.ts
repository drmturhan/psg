import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-anasayfa',
  templateUrl: './anasayfa.component.html',
  styleUrls: ['./anasayfa.component.css']
})
export class AnasayfaComponent implements OnInit {

  registerMode: boolean = false;
  constructor(public authService: AuthService) { }

  ngOnInit() {
    this.authService.suankiFotoUrl.subscribe(() => this.registerMode = false);
  }
  registerToogle() {
    this.registerMode = !this.registerMode;
  }
  uyeligiIptalEt(durum: boolean) {
    this.registerMode = durum;
  }

}
