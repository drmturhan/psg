import { Component, OnInit, Input } from '@angular/core';
import { MesajListe, MesajYaratma } from '../../_models/mesaj-liste';
import { ListeSonuc } from '../../_models/sonuc';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { MesajlasmaService } from '../../_services/mesajlasma.service';
import 'rxjs/add/operator/do';
import * as _ from 'underscore';
@Component({
  selector: 'app-kullanici-mesaj',
  templateUrl: './kullanici-mesaj.component.html',
  styleUrls: ['./kullanici-mesaj.component.css']
})
export class KullaniciMesajComponent implements OnInit {

  @Input() kullaniciNo: number;
  mesajlar: ListeSonuc<MesajListe>;
  yeniMesaj = new MesajYaratma();
  constructor(
    private authService: AuthService,
    private mesajlasmaService: MesajlasmaService,
    private uyarici: AlertifyService) { }

  ngOnInit() {
    this.mesajlariYukle();
  }

  mesajlariYukle() {
    const kullaniciNo = +this.authService.suankiKullanici.id;
    this.mesajlasmaService.mesajYiginiGetir(this.authService.suankiKullanici.id, this.kullaniciNo)
      .do(gelenMesajlar => {
        _.each(gelenMesajlar.donenListe, (mesaj: MesajListe) => {
          if (mesaj.okundu === false && mesaj.alanNo === kullaniciNo) {
            this.mesajlasmaService.okunduOlarakIsaretle(kullaniciNo, mesaj.mesajId).subscribe();
          }
        });
      })
      .subscribe((sonuc: ListeSonuc<MesajListe>) => {
        this.mesajlar = sonuc;
      });
  }

  mesajGonder() {
    this.yeniMesaj.alanNo = this.kullaniciNo;
    this.yeniMesaj.gonderenNo = this.authService.suankiKullanici.id;
    this.mesajlasmaService.mesajGonder(this.yeniMesaj).subscribe((eklenenMesaj: MesajListe) => {
      this.mesajlar.donenListe.unshift(eklenenMesaj);
      this.yeniMesaj = new MesajYaratma();
    },
      hata => {
        this.uyarici.error('Mesaj gönderilemedi!');
      });
  }
}
