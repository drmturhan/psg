import { AlertifyService } from './../../_services/alertify.service';
import { KullaniciService } from './../../_services/kullanici.service';
import { Component, OnInit } from '@angular/core';
import { Kullanici } from '../../_models/kullanici';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Foto } from '../../_models/foto';
@Component({
  selector: 'app-kullanici-detay',
  templateUrl: './kullanici-detay.component.html',
  styleUrls: ['./kullanici-detay.component.css']
})
export class KullaniciDetayComponent implements OnInit {

  kullanici: Kullanici;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private kulServisi: KullaniciService,
    private uyarici: AlertifyService,
    private acRoute: ActivatedRoute) { }

  ngOnInit() {
    this.acRoute.data.subscribe(data => {
      this.kullanici = data['kullanici'];
    })
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false

    }]
    this.galleryImages = this.getImages();
  }
  getImages():NgxGalleryImage[] {
    var imageUrls: NgxGalleryImage[] = [];
    for (let i = 0; i < this.kullanici.fotograflari.length; i++) {
      var yeni:NgxGalleryImage={};
      var item=this.kullanici.fotograflari[i];
      yeni.small=item.url;
      yeni.medium=item.url;
      yeni.big=item.url;
      yeni.description=item.aciklama;
      yeni.url=item.url;
      imageUrls.push(yeni);
    }
    return imageUrls;

  }
}
