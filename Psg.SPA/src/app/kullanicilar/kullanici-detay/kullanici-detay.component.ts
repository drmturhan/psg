import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { Kullanici } from '../../_models/kullanici';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Foto } from '../../_models/foto';
import { environment } from '../../../environments/environment';
import { KullaniciService } from '../../_services/kullanici.service';
import { NotFoundError } from '../../_hatalar/not-found-error';
import { AppError } from '../../_hatalar/app-error';
@Component({
  selector: 'app-kullanici-detay',
  templateUrl: './kullanici-detay.component.html',
  styleUrls: ['./kullanici-detay.component.css']
})
export class KullaniciDetayComponent implements OnInit {

  kullanici: Kullanici;
  profilFotoUrl: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(
    private uyarici: AlertifyService,
    private kullaniciService: KullaniciService,
    private acRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.acRoute.data.subscribe(data => {
      if (data['kullanici'].basarili) {
        this.kullanici = data['kullanici'].donenNesne;

        if (this.kullanici.profilFotoUrl) {
          this.profilFotoUrl = this.kullanici.profilFotoUrl;
        } else {
          this.profilFotoUrl = environment.bosFotoUrl;
        }
      }
    });
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
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
  silmeOnayiIste(kullanici: Kullanici) {
    this.uyarici.confirm('Bu kullanıcıyı silmek istediğinizden emin misiniz?',
      () => {
        this.sil(kullanici.id);
      }, 'Emin misiniz?', 'Evet', 'Hayır'
    );
  }
  sil(id: number) {
    this.kullaniciService.delete(id)
      .subscribe(() => {
        this.uyarici.success('Kullanıcı silindi!');
        this.router.navigate(['/kullanicilar']);
      },
      (hata: AppError) => {
        if (hata instanceof NotFoundError) {
        } else {
          throw hata;
        }
      });
  }
}
