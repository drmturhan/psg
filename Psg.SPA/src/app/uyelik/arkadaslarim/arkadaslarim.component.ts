import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ListeSonuc } from './../../_models/sonuc';
import { ArkadasliklarimListe } from './../../_models/arkadaslik-liste';
import { ArkadaslikService } from '../../_services/arkadaslik.service';
import { ArkadaslikSorgusu } from '../../_models/sorgular/arkadaslik-sorgusu';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-arkadaslarim',
  templateUrl: './arkadaslarim.component.html',
  styleUrls: ['./arkadaslarim.component.css']
})
export class ArkadaslarimComponent implements OnInit {
  sorgu: ArkadaslikSorgusu = {};
  filtre: any;
  constructor(private route: ActivatedRoute, private arkadaslikService: ArkadaslikService, private authService: AuthService) { }
  arkadasliklarim: ListeSonuc<ArkadasliklarimListe> = new ListeSonuc<ArkadasliklarimListe>();
  gosterilenArkadasliklarim: ListeSonuc<ArkadasliklarimListe> = new ListeSonuc<ArkadasliklarimListe>();
  kullaniciNumaram: number;
  ngOnInit() {
    this.kullaniciNumaram = this.authService.suankiKullanici.id;
    this.route.data.subscribe((data: ListeSonuc<ArkadasliklarimListe>) => {

      const kullaniciVeriSeti = data['arkadaslarim'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.basarili) {
        this.arkadasliklarim = kullaniciVeriSeti;
        this.filtre = {
          gelenTeklifler: true,
          gidenTeklifler: false
        };
        this.filtrele();
      }
    });
  }
  pageChanged(event: any): void {

    this.arkadaslikService.arkadasliklariGetir(this.sorgu).subscribe((sonuc: ListeSonuc<ArkadasliklarimListe>) => {
      this.arkadasliklarim = sonuc;
    });
  }
  filtrele() {
    if (this.filtre.gelenTeklifler) {
      this.gosterilenArkadasliklarim.donenListe = this.arkadasliklarim
        .donenListe.filter(f => f.teklifEdilen != null && f.teklifEdilen.id === this.kullaniciNumaram);
    }
    if (this.filtre.gidenTeklifler) {
      this.gosterilenArkadasliklarim.donenListe = this.arkadasliklarim
        .donenListe.filter(f => f.teklifEden != null && f.teklifEden.id === this.kullaniciNumaram);
    }
  }

}
