import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ArkadasliklarimListe } from '../../../_models/arkadaslik-liste';
import { AlertifyService } from '../../../_services/alertify.service';
import { ArkadaslikService } from '../../../_services/arkadaslik.service';
import { BadInputError } from '../../../_hatalar/bad-input';
import { AppError } from '../../../_hatalar/app-error';
import { Router } from '@angular/router';
import { AuthService } from '../../../_services/auth.service';


@Component({
  selector: 'app-arkadas-karti',
  templateUrl: './arkadas-karti.component.html',
  styleUrls: ['./arkadas-karti.component.css']
})
export class ArkadasKartiComponent implements OnInit {

  @Input() arkadaslik: ArkadasliklarimListe;
  @Input() sahipKullaniciNo: number;
  @Output() listeyiYenile = new EventEmitter();
  constructor(
    private uyarici: AlertifyService,
    private arkadaslikService: ArkadaslikService,
    private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
  }
  teklifIptalEt(teklif: ArkadasliklarimListe) {
    this.arkadaslikService.arkadaslikTeklifiniIptalEt(teklif.teklifEden.id, teklif.teklifEdilen.id)
      .subscribe(data => {
        this.listeyiYenile.emit();
        this.uyarici.success('İptal edildi');
      },
        (hata: AppError) => {
          if (hata instanceof BadInputError) {
            this.uyarici.error(hata.orjinalHata);
          } else {
            throw hata;
          }
        });
  }
  teklifiKabulEt(teklif: ArkadasliklarimListe) {
    this.arkadaslikService.arkadaslikTeklifineKararVer(teklif.teklifEden.id, teklif.teklifEdilen.id, true)
      .subscribe(data => {
        this.listeyiYenile.emit();
        this.uyarici.success('Teklif kabul edildi.');
      },
        (hata: AppError) => {
          if (hata instanceof BadInputError) {
            this.uyarici.error(hata.orjinalHata);
          } else {
            throw hata;
          }
        });
  }
  teklifiReddet(teklif: ArkadasliklarimListe) {
    this.arkadaslikService.arkadaslikTeklifineKararVer(teklif.teklifEden.id, teklif.teklifEdilen.id, false)
      .subscribe(data => {
        this.listeyiYenile.emit();
        this.uyarici.success('Teklif kabul edildi.');
      },
        (hata: AppError) => {
          if (hata instanceof BadInputError) {
            this.uyarici.error(hata.orjinalHata);
          } else {
            throw hata;
          }
        });
  }
  mesajlasmayiBaslat(teklif: ArkadasliklarimListe) {
    let kullaniciNo = 0;
    if (teklif.teklifEden.id !== this.authService.suankiKullanici.id) {
      kullaniciNo = teklif.teklifEden.id;
    } else {
      kullaniciNo = teklif.teklifEdilen.id;
    }
    this.router.navigate(['/kullanicilar/', kullaniciNo], { queryParams: { sayfa: ' 1' } });
  }
}
