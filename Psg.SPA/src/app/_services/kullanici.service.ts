import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { ArkadaslikListe } from '../_models/arkadaslik-liste';
import { Observable } from 'rxjs/Observable';
import { Kullanici, KullaniciYaz } from '../_models/kullanici';


@Injectable()
export class KullaniciService extends DataService {

  constructor(http: HttpClient) {
    super('kullanicilar',  http);
  }
  arkadaslikteklifEt(isteyenId: number, cevaplayanId: number) {
    return this.create({}, `kullanicilar/${isteyenId}/teklif/${cevaplayanId}`);
  }

  arkadasliklariGetir(): Observable<ListeSonuc<ArkadaslikListe>> {
    return this.list('arkadasliklar')
      .map(
      response => {
        return <ListeSonuc<ArkadaslikListe>>response;
      });
  }
  kullaniciBul(id: number): Observable<KayitSonuc<Kullanici>> {
    return this.get(id)
      .map(
      response => {
        return <KayitSonuc<Kullanici>>response;
      });
  }
  kullaniciBulDegistirmekIcin(id: number): Observable<KayitSonuc<KullaniciYaz>> {
    return this.get(0, `kullanicilar/${id}?neden=yaz`)
      .map(
      response => {
        return <KayitSonuc<KullaniciYaz>>response;
      });
  }
  asilFotoYap(kullaniciNo: number, fotoId: number) {
    return this.create(`kullanicilar/${kullaniciNo}/fotograflari/${fotoId}/asilYap`);
  }
  fotografSil(kullaniciNo: number, fotoId: number) {
    return this.delete('kullanicilar/' + kullaniciNo + '/fotograflari/' + fotoId)
      .catch(this.handleError);
  }
  kullaniciVar(kullaniciAdi): Observable<boolean> {
    return this.get(0, 'kullanicilar/kullaniciAdiVar/?kullaniciAdi=' + kullaniciAdi)
      .map(response => response)
      .catch(this.handleError);
  }
}