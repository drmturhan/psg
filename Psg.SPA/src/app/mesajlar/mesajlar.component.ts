import { Component, OnInit } from '@angular/core';
import { MesajListe } from '../_models/mesaj-liste';
import { MesajlasmaSorgusu } from '../_models/sorgular/mesajlar-sorgusu';
import { ListeSonuc } from '../_models/sonuc';
import { AlertifyService } from '../_services/alertify.service';
import { MesajlasmaService } from '../_services/mesajlasma.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';

import * as _ from 'underscore';
@Component({
  selector: 'app-mesajlar',
  templateUrl: './mesajlar.component.html',
  styleUrls: ['./mesajlar.component.css']
})
export class MesajlarComponent implements OnInit {

  mesajlar: ListeSonuc<MesajListe>;
  sorgu: MesajlasmaSorgusu = new MesajlasmaSorgusu();
  constructor(
    private uyarici: AlertifyService,
    private mesajlasmaService: MesajlasmaService,
    private authService: AuthService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((data: ListeSonuc<MesajListe>) => {
      this.mesajlar = data['mesajlarim'];
    });
  }
  yukle() {
    const kullaniciNo = +this.authService.suankiKullanici.id;
    this.mesajlasmaService.listeGetirMesajlar(kullaniciNo, this.sorgu)     
      .subscribe(data => {
        this.mesajlar = data;
      });
  }
  mesajSil(mesajId: number) {
    this.uyarici.confirm('Mesajı silmek üzeresiniz. Silinen mesaj geri alınamaz. Mesaj silinsin mi?',
      () => {
        this.mesajlasmaService.mesajSil(mesajId).subscribe(() => {
          this.mesajlar.donenListe.splice(_.findIndex(this.mesajlar.donenListe, { mesajId: mesajId }), 1);
          this.uyarici.success('Mesaj silindi!');
        }, hata => { this.uyarici.error('Mesaj silinemedi!'); });
      },
      null,
      'Emin misiniz?', 'Evet', 'Hayır'
    );
  }
  okunmamisMesalariYukle() {
    this.sorgu.gidenMesajlar = false;
    this.yukle();
  }

  gelenMesajlariYukle() {
    this.sorgu.gidenMesajlar = false;
    this.yukle();
  }
  gidenKutusunuYukle() {
    this.sorgu.gelenMesajlar = false;
    this.yukle();
  }
  pageChanged(event: any): void {
    this.sorgu.sayfa = event.page;
    this.yukle();
  }
}
