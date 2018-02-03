import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../../environments/environment';
import { KisiFoto } from './../../../_models/foto';
import { AuthService } from '../../../_services/auth.service';
import { KullaniciService } from '../../../_services/kullanici.service';
import { AlertifyService } from '../../../_services/alertify.service';
import * as _ from 'underscore';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { AppError } from '../../../_hatalar/app-error';
@Component({
  selector: 'app-fotograf-duzenleyici',
  templateUrl: './fotograf-duzenleyici.component.html',
  styleUrls: ['./fotograf-duzenleyici.component.css']
})
export class FotografDuzenleyiciComponent implements OnInit {
  @Input() fotograflar: KisiFoto[];
  @Input() url: string;
  @Output() profilFotografiYap = new EventEmitter<KisiFoto>();
  @Output() fotoSil = new EventEmitter<number>();
  @Output() fotografKaydedildi = new EventEmitter<KisiFoto>();

  uploader: FileUploader = new FileUploader({});

  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService,
    private kullaniciService: KullaniciService,
    private uyarici: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  initializeUploader() {
    const kullaniciNo = this.authService.suankiKullanici.id;
    const jeton = 'Bearer ' + localStorage.getItem('access_token');
    if (this.url == null) {
      throw new AppError('Url yok');
    }
    this.uploader = new FileUploader({
      url: `${this.baseUrl}/${this.url}`,
      authToken: jeton,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 102,
    });

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: KisiFoto = JSON.parse(response);
        const foto = {
          id: res.id,
          url: res.url,
          kisiNo: res.kisiNo,
          aciklama: res.aciklama,
          eklemeTarihi: res.eklenmeTarihi,
          profilFotografi: res.profilFotografi
        };
        this.fotograflar.push(foto);
        this.fotografKaydedildi.emit(foto);
      }
    };
    this.uploader.onErrorItem = (item, response, status, headers) => {
      this.uploader.cancelItem(item);
      this.uyarici.error('Fotoğraf yüklenemedi!');
    };
    this.uploader.onCompleteAll = () => {
      this.uploader.clearQueue();

    };
  }
  asilFotoYap(foto: KisiFoto) {
    this.profilFotografiYap.emit(foto);
  }
  silmeOnayiIste(foto: KisiFoto) {
    this.uyarici.confirm('Bu fotoğrafı silmek istediğinizden emin misiniz?',
      () => {
        this.fotoSil.emit(foto.id);
      }, 'Emin misiniz?', 'Evet', 'Hayır'
    );
  }
}

