import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { Observable } from 'rxjs/Observable';
import { Kullanici, KullaniciYaz } from '../_models/kullanici';
import { environment } from '../../environments/environment';
import { KullaniciSorgusu } from '../_models/sorgular/kullanici-sorgusu';



@Injectable()
export class KullaniciService {
  constructor(private dataService: HttpClient) {
    this.url = environment.apiUrl;
  }
  private url: string;

  
  listeGetirKullanicilar(sorgu?: KullaniciSorgusu) {
    if (sorgu == null) {
      sorgu = new KullaniciSorgusu();
      sorgu.siralamaCumlesi = 'AdSoyad';
      sorgu.aramaCumlesi = '';
      sorgu.sayfa = 1;
      sorgu.sayfaBuyuklugu = 10;
    }
    let params = new HttpParams();
    if (sorgu.aramaCumlesi != null) {
      params = params.append('aramaCumlesi', sorgu.aramaCumlesi);
    }
    if (sorgu.sayfa != null) {
      params = params.append('sayfa', sorgu.sayfa.toString());
    }
    if (sorgu.sayfaBuyuklugu != null) {
      params = params.append('sayfaBuyuklugu', sorgu.sayfaBuyuklugu.toString());
    }
    if (sorgu.siralamaCumlesi != null) {
      params = params.append('siralamaCumlesi', sorgu.siralamaCumlesi.toString());
    }
    return this.dataService.get<ListeSonuc<Kullanici>>(`${environment.apiUrl}kullanicilar`, { params: params });

  }



  delete(id: number) {
    return this.dataService.delete(`${this.url}kullanicilar/${id}`);
  }
  update(id: number, kullanici: KullaniciYaz) {
    return this.dataService.put<Kullanici>(`${environment.apiUrl}kullanicilar/${id}`, kullanici);
  }
  kullaniciAdiKullanimda(kullaniciAdi: string) {
    return this.dataService.get(`${this.url}kullanicilar/kullaniciadikullanimda?kullaniciAdi=${kullaniciAdi}`);
  }
  mailAdresiKullanimda(eposta: string) {
    return this.dataService.get(`${this.url}kullanicilar/epostakullanimda?eposta=${eposta}`);
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