
import { KullaniciService } from './../../_services/kullanici.service';
import { Kullanici } from './../../_models/kullanici';
import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { environment } from '../../../environments/environment';
import { AlertifyService } from '../../_services/alertify.service';
import { AppError } from '../../_hatalar/app-error';
import { BadInputError } from '../../_hatalar/bad-input';
import { ArkadaslikService } from '../../_services/arkadaslik.service';

@Component({
  selector: 'app-kullanici-karti',
  templateUrl: './kullanici-karti.component.html',
  styleUrls: ['./kullanici-karti.component.css']
})
export class KullaniciKartiComponent implements OnInit {
  @Input() kullanici: Kullanici;
  bosFotoUrl: string = environment.bosFotoUrl;
  constructor(public authService: AuthService, private arkadaslikService: ArkadaslikService,
    private uyarici: AlertifyService
  ) { }
  ngOnInit() {

  }
  teklifEt(id: number) {
    this.arkadaslikService.arkadaslikteklifEt(this.authService.suankiKullanici.id, id)
      .subscribe(data => {
        this.uyarici.success(`${this.kullanici.tamAdi} adlı kullanıcıya arkadaşlık isteği gönderildi!`);
      },
      (hata: AppError) => {
        if (hata instanceof BadInputError) {
          this.uyarici.error(hata.orjinalHata);
        } else {
          throw hata;
        }
      });
  }
}
