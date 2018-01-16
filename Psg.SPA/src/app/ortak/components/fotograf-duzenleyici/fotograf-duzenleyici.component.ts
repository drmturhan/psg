import { Foto } from './../../../_models/foto';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../_services/auth.service';
import { KullaniciService } from '../../../_services/kullanici.service';
import { AlertifyService } from '../../../_services/alertify.service';
import * as _ from "underscore";
@Component({
  selector: 'fotograf-duzenleyici',
  templateUrl: './fotograf-duzenleyici.component.html',
  styleUrls: ['./fotograf-duzenleyici.component.css']
})
export class FotografDuzenleyiciComponent implements OnInit {
  @Input() fotograflar: Foto[];
  @Output() asilFotoDegisti = new EventEmitter<string>();
  uploader: FileUploader = new FileUploader({});

  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  suankiAsil: Foto;
  constructor(private authService: AuthService,
    private kullaniciService: KullaniciService,
    private uyarici: AlertifyService) { }

  ngOnInit() {
    console.log(this.fotograflar);
    this.initializeUploader();
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'kullanicilar/' + this.authService.kullaniciNumarasiniAl() + '/fotograflari',
      authToken: 'Bearer ' + localStorage.getItem('access_token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Foto = JSON.parse(response);
        const foto = {
          id: res.id,
          url: res.url,
          aciklama: res.aciklama,
          eklemeTarihi: res.eklenmeTarihi,
          ilkTercih: res.profilFotografi
        }
        this.fotograflar.push(foto);
        if (foto.ilkTercih) {
          this.fotoUrlAyarla(foto.url);
        }
      };
    }
  }
  public fotoUrlAyarla(fotoUrl: string) {
    let url = environment.bosFotoUrl;
    if (this.authService.suankiKullanici.profilFotoUrl !== '')
      url = this.authService.suankiKullanici.profilFotoUrl;
    this.authService.kullaniciFotografiniDegistir(fotoUrl);
  }
  asilFotoYap(foto: Foto) {
    this.kullaniciService.asilFotoYap(this.authService.kullaniciNumarasiniAl(), foto.id)
      .subscribe(() => {
        this.suankiAsil = _.findWhere(this.fotograflar, { profilFotografi: true })
        this.suankiAsil.profilFotografi = false;
        foto.profilFotografi = true;
        this.fotoUrlAyarla(foto.url);
        this.authService.suankiKullanici.profilFotoUrl = foto.url;
        localStorage.setItem('kullanici', JSON.stringify(this.authService.suankiKullanici));
        this.uyarici.success('Asıl foto yapıldı.')
      },
      hata => this.uyarici.error('Asıl foto yapılırken bir hata oluştu!'))
  }
  silmeOnayiIste(foto: Foto) {
    this.uyarici.confirm('Bu fotoğrafı silmek istediğinizden emin misiniz?',
      () => {
        this.sil(foto.id);
      }, 'Emin misiniz?', 'Evet', 'Hayır'
    );
  }
  sil(id: number) {
    this.kullaniciService.sil(this.authService.kullaniciNumarasiniAl(), id)
      .subscribe(() => {
        this.fotograflar.splice(_.findIndex(this.fotograflar, { id: id }), 1);
        this.uyarici.success("Fotoğraf silindi!");
      },
      hata => this.uyarici.error("Fotoğraf silinemedi!")
      );

  }
}

