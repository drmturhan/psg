import { ActivatedRoute } from '@angular/router';
import { Kullanici } from './../../_models/kullanici';
import { Component, OnInit } from '@angular/core';
import { KullaniciService } from '../../_services/kullanici.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-kullanici-listesi',
  templateUrl: './kullanici-listesi.component.html',
  styleUrls: ['./kullanici-listesi.component.css']
})
export class KullaniciListesiComponent implements OnInit {

  kullanicilar: Kullanici[]
  constructor(
    private uyarici: AlertifyService,
    private acRoute: ActivatedRoute) { }

  ngOnInit() {
    this.acRoute.data.subscribe(data => {
      this.kullanicilar = data['kullanicilar'];
    })
  }
}
