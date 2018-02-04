

export class KayitSonuc<T>
{
  basarili: boolean;
  hatalar?: string[];
  mesajlar?: string[];
  donenNesne: T;
  donenSekillenmisNesne: any;

}

export class ListeSonuc<T>
{
  basarili: boolean;
  donenListe: T[];
  donenSekillenmisListe: any[];
  hatalar?: string[];
  mesajlar?: string[];
  kayitSayisi: number;
  sayfaSayisi: number;
  sayfaBuyuklugu: number;
  sayfa: number;
}

