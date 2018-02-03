import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Component, OnInit, ViewChild, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { KullaniciYaz } from '../../_models/kullanici';
import { AlertifyService } from '../../_services/alertify.service';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/bs-moment';
import { tr } from 'ngx-bootstrap/locale';
import { ListeSonuc } from './../../_models/sonuc';
import { Cinsiyet, KisiFoto } from './../../_models/foto';
import { AuthService } from './../../_services/auth.service';
import { KullaniciService } from './../../_services/kullanici.service';
import * as _ from 'underscore';
@Component({
  selector: 'app-profilim',
  templateUrl: './profilim.component.html',
  styleUrls: ['./profilim.component.css']
})
export class ProfilimComponent implements OnInit {

  @ViewChild('editForm') duzenlemeFormu;
  bsConfig: Partial<BsDatepickerConfig>;
  kullanici: KullaniciYaz;
  cinsiyetler: Cinsiyet[];
  saveUrl = '';
  suankiProfilFotografi: KisiFoto;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];


  fotoUrl: string;
  constructor(
    private route: ActivatedRoute,
    private kullaniciService: KullaniciService,
    private authService: AuthService,
    private uyarici: AlertifyService,
    private _localeService: BsLocaleService) { }

  ngOnInit() {
    this.authService.suankiFotoUrl.subscribe(fotoUrl => this.fotoUrl = fotoUrl);
    defineLocale('tr', tr);
    this._localeService.use('tr');
    this.bsConfig = { containerClass: 'theme-red' };
    this.route.data.subscribe(data => {
      const kullaniciVeriSeti = data['kullaniciVeriSeti'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.kullaniciSonuc.basarili) {
        this.kullanici = kullaniciVeriSeti.kullaniciSonuc.donenNesne;
        this.cinsiyetler = kullaniciVeriSeti.cinsiyetler;
        this.saveUrl = `profilim/fotografEkle`;
      }
    });

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
    const id = this.authService.suankiKullanici.id;
    this.kullaniciService.update(id, this.kullanici).subscribe(
      next => {
        this.duzenlemeFormu.reset(this.kullanici);
        this.uyarici.success('Kaydedildi');
      },
      hata => {

      });
  }
  asilFotoDegisti(url: string) {
    this.kullanici.profilFotoUrl = url;
    this.authService.profilFotografiDegisti(url);
  }
  profilFotografiYap(foto: KisiFoto) {

    this.authService.profilFotografiYap(foto.id)
      .subscribe(() => {
        this.suankiProfilFotografi = _.findWhere(this.kullanici.fotograflari, { profilFotografi: true });
        this.suankiProfilFotografi.profilFotografi = false;
        foto.profilFotografi = true;
        this.authService.profilFotografiDegisti(foto.url);
        this.uyarici.success('Asıl foto yapıldı.');
      },
      hata => this.uyarici.error('Asıl foto yapılırken bir hata oluştu!'));
  }
  fotografKaydedildi(foto: KisiFoto) {
    if (this.authService.suankiKullanici == null) {
      this.authService.suankiKullanici.fotograflari = [];
      this.authService.suankiKullanici.fotograflari.push(foto);
    }
    if (foto.profilFotografi === true) {
      this.authService.profilFotografiDegisti(foto.url);
    }
  }
  fotograSil(id: number) {
    this.authService.fotografSil(id)
      .subscribe(() => {
        this.kullanici.fotograflari.splice(_.findIndex(this.kullanici.fotograflari, { id: id }), 1);
        this.uyarici.success('Fotoğraf silindi!');
      },
      hata => this.uyarici.error('Fotoğraf silinemedi!')
      );
  }
}
