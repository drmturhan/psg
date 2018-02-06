import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { KullaniciService } from '../../_services/kullanici.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ListeSonuc } from '../../_models/sonuc';
import { KullaniciBilgi } from '../../_models/kullanici';
import { AuthService } from '../../_services/auth.service';
import { KullaniciSorgusu } from '../../_models/sorgular/kullanici-sorgusu';

@Component({
  selector: 'app-kullanici-listesi',
  templateUrl: './kullanici-listesi.component.html',
  styleUrls: ['./kullanici-listesi.component.css']
})
export class KullaniciListesiComponent implements OnInit {

  kullanici: KullaniciBilgi;
  kullanicilar: ListeSonuc<KullaniciBilgi>;
  sorgu: KullaniciSorgusu = new KullaniciSorgusu();
  constructor(
    private kullaniciService: KullaniciService,
    private authService: AuthService,
    private uyarici: AlertifyService,
    private acRoute: ActivatedRoute) { }

  ngOnInit() {
    this.sorgu.siralamaCumlesi = 'AdSoyad';
    this.kullanici = this.authService.suankiKullanici;
    this.acRoute.data.subscribe(data => {
      this.kullanicilar = data['kullanicilar']['kullaniciSonuc'];
      this.sorgu.sayfa = this.kullanicilar.sayfa;
      this.sorgu.sayfaBuyuklugu = this.kullanicilar.sayfaBuyuklugu;
      this.sorgu.siralamaCumlesi = 'AdSoyad';
    });
  }
  kullanicilariYukle() {
    this.kullaniciService.listeGetirKullanicilar(this.sorgu).subscribe(veri => {
      this.kullanicilar = veri;
    });
  }
  sorguyuSifirla() {
    this.sorgu.sayfa = 1;
    this.sorgu.aramaCumlesi = null;
    this.sorgu.siralamaCumlesi = 'AdSoyad';
  }
  tumKullanicilariYukle() {
    this.sorguyuSifirla();
    this.kullanicilariYukle();
  }
  pageChanged(event: any): void {
    this.sorgu.sayfa = event.page;
    this.kullanicilariYukle();
  }
}
