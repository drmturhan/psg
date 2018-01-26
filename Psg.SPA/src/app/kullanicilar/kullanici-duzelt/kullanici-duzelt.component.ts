import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Component, OnInit, ViewChild, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { KullaniciYaz } from '../../_models/kullanici';
import { AlertifyService } from '../../_services/alertify.service';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/bs-moment';
import { tr } from 'ngx-bootstrap/locale';
import { ListeSonuc } from './../../_models/sonuc';
import { Cinsiyet } from './../../_models/foto';
import { AuthService } from './../../_services/auth.service';
import { KullaniciService } from './../../_services/kullanici.service';
@Component({
  selector: 'app-kullanici-duzelt',
  templateUrl: './kullanici-duzelt.component.html',
  styleUrls: ['./kullanici-duzelt.component.css']
})
export class KullaniciDuzeltComponent implements OnInit {

  @ViewChild('editForm') duzenlemeFormu;
  bsConfig: Partial<BsDatepickerConfig>;
  kullanici: KullaniciYaz;
  cinsiyetler: Cinsiyet[];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];


  fotoUrl: string;
  constructor(
    private route: ActivatedRoute,
    private kullaniciService: KullaniciService,
    private auth: AuthService,
    private uyarici: AlertifyService,
    private _localeService: BsLocaleService) { }

  ngOnInit() {
    defineLocale('tr', tr);
    this._localeService.use('tr');
    this.bsConfig = { containerClass: 'theme-red' };
    this.route.data.subscribe(data => {
      const kullaniciVeriSeti = data['kullaniciVeriSeti'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.kullaniciSonuc.basarili) {
        this.kullanici = kullaniciVeriSeti.kullaniciSonuc.donenNesne;
        this.cinsiyetler = kullaniciVeriSeti.cinsiyetler;
      }
    });
    this.auth.suankiFotoUrl.subscribe(fotoUrl => this.fotoUrl = fotoUrl);
    this.galleryOptions = [{
      width: '100%',
      height: '100%',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false

    }];
    this.galleryImages = this.getImages();
  }


  getImages(): NgxGalleryImage[] {
    const imageUrls: NgxGalleryImage[] = [];
    for (let i = 0; i < this.kullanici.fotograflari.length; i++) {
      const yeni: NgxGalleryImage = {};
      const item = this.kullanici.fotograflari[i];
      yeni.small = item.url;
      yeni.medium = item.url;
      yeni.big = item.url;
      yeni.description = item.aciklama;
      yeni.url = item.url;
      imageUrls.push(yeni);
    }
    return imageUrls;
  }

  kaydet() {
    const id = this.auth.kullaniciNumarasiniAl();
    this.kullaniciService.update(id, this.kullanici).subscribe(
      next => {
        this.duzenlemeFormu.reset(this.kullanici);
        this.uyarici.success('Kaydedildi');
      });
  }
  asilFotoDegisti(url: string) {
    this.kullanici.profilFotoUrl = url;
    this.auth.kullaniciFotografiniDegistir(url);
  }
}
