import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { ArkadaslikListe } from '../_models/arkadaslik-liste';
import { Observable } from 'rxjs/Observable';
import { Kullanici, KullaniciYaz } from '../_models/kullanici';
import { environment } from '../../environments/environment';


@Injectable()
export class KullaniciService {
  constructor(private dataService: DataService) {
    this.url = environment.apiUrl;
  }
  private url: string;

  arkadaslikteklifEt(isteyenId: number, cevaplayanId: number) {
    return this.dataService.get(`${this.url} kullanicilar/${isteyenId}/teklif/${cevaplayanId}`);
  }
  listeGetirKullanicilar() {
    return this.dataService.get<ListeSonuc<Kullanici>>(`${environment.apiUrl}kullanicilar`);

  }
  delete(id: number) {
    return this.dataService.delete(`${this.url}kullanicilar/${id}`);
  }
  update(id: number, kullanici: KullaniciYaz) {
    return this.dataService.kaydet<Kullanici>(`${environment.apiUrl}kullanicilar/${id}`, kullanici);
  }
  kullaniciAdiVar(kullaniciAdi: string) {
    return this.dataService.get(`${this.url}?kullaniciAdi=${kullaniciAdi}`);
  }
  arkadasliklariGetir(): Observable<ListeSonuc<ArkadaslikListe>> {
    return this.dataService.get<ListeSonuc<ArkadaslikListe>>(`${this.url}arkadasliklar`)
      .map(
      response => {
        return <ListeSonuc<ArkadaslikListe>>response;
      });
  }
  kullaniciBul(id: number): Observable<KayitSonuc<Kullanici>> {
    return this.dataService.get<KayitSonuc<Kullanici>>(`${this.url}kullanicilar/${id}`)
      .map(
      response => {
        return <KayitSonuc<Kullanici>>response;
      });
  }
  kullaniciBulDegistirmekIcin(id: number): Observable<KayitSonuc<KullaniciYaz>> {
    return this.dataService.get<KayitSonuc<KullaniciYaz>>(`${this.url}kullanicilar/${id}?neden=yaz`)
      .map(
      response => {
        return <KayitSonuc<KullaniciYaz>>response;
      });
  }
  asilFotoYap(kullaniciNo: number, fotoId: number) {
    return this.dataService.get(`${this.url}kullanicilar/${kullaniciNo}/fotograflari/${fotoId}/asilYap`);
  }
  fotografSil(kullaniciNo: number, fotoId: number) {
    return this.dataService.delete(`${this.url}kullanicilar/${kullaniciNo}/fotograflari/${fotoId}`);
  }
}