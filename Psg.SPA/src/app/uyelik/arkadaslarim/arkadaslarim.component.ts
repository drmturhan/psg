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
  sorgu: ArkadaslikSorgusu;
  constructor(private route: ActivatedRoute,
    private arkadaslikService: ArkadaslikService, public authService: AuthService) {
    this.sorgu = new ArkadaslikSorgusu();
    this.sorgu.sayfaBuyuklugu = 10;
  }
  gosterilenArkadasliklarim: ListeSonuc<ArkadasliklarimListe> = new ListeSonuc<ArkadasliklarimListe>();
  kullaniciNumaram: number;
  ngOnInit() {
    this.kullaniciNumaram = this.authService.suankiKullanici.id;
    this.route.data.subscribe((data: ListeSonuc<ArkadasliklarimListe>) => {
      const kullaniciVeriSeti = data['arkadaslarim'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.basarili) {
        this.gosterilenArkadasliklarim = kullaniciVeriSeti;
        this.sorguyuAyarla(this.gosterilenArkadasliklarim);
      }
    });
  }
  sorguyuAyarla(sonuc: ListeSonuc<ArkadasliklarimListe>) {
    this.sorgu.sayfaBuyuklugu = sonuc.sayfaBuyuklugu;
    this.sorgu.sayfa = sonuc.sayfa;
    this.sorgu.sayfaSayisi = sonuc.sayfaSayisi;
    this.sorgu.kayitSayisi = sonuc.kayitSayisi;
  }
  teklifEdilenler() {
    if (this.sorgu.teklifEdilenler === true) {
      this.sorgu.teklifEdenler = false;
    }
    this.yukle();
  }
  teklifEdenler() {
    if (this.sorgu.teklifEdenler === true) {
      this.sorgu.teklifEdilenler = false;
    }
    this.yukle();
  }
  kabulEdilenler() {
    if (this.sorgu.kabulEdilenler === true) {
      this.sorgu.cevapBeklenenler = false;
    }
    this.yukle();
  }
  cevaplananlar() {
    if (this.sorgu.cevaplananlar === true) {
      this.sorgu.cevapBeklenenler = false;
    }
    this.yukle();
  }
  cevapBekleyenler() {
    if (this.sorgu.cevapBeklenenler === true) {
      this.sorgu.kabulEdilenler = false;
      this.sorgu.cevaplananlar = false;
    }
    this.yukle();
  }

  yukle() {
    this.arkadaslikService.arkadasliklariGetir(this.sorgu).subscribe((kayitlar: ListeSonuc<ArkadasliklarimListe>) => {
      this.gosterilenArkadasliklarim = kayitlar;
      this.sorguyuAyarla(this.gosterilenArkadasliklarim);
    });

  }
  pageChanged(event: any): void {
    this.sorgu.sayfa = event.page;
    this.yukle();
  }
  listeyiYenile(durum) {
    this.yukle();
  }
}
