import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { KullaniciService } from '../../_services/kullanici.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ArkadaslikListe } from '../../_models/arkadaslik-liste';
import { ListeSonuc } from '../../_models/sonuc';

@Component({
  selector: 'app-kullanici-listesi',
  templateUrl: './kullanici-listesi.component.html',
  styleUrls: ['./kullanici-listesi.component.css']
})
export class KullaniciListesiComponent implements OnInit {

  kullanicilar: ListeSonuc<ArkadaslikListe>;
  constructor(
    private uyarici: AlertifyService,
    private acRoute: ActivatedRoute) { }

  ngOnInit() {
    this.acRoute.data.subscribe(data => {
      this.kullanicilar = data['kullanicilar']['kullaniciSonuc'];
    });
  }
}
