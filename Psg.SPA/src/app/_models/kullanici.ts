import { Foto, Cinsiyet } from './foto';
import { ListeSonuc, KayitSonuc } from './sonuc';
export class Kullanici {
    id?: number;
    kullaniciAdi?: string;
    tamAdi?: string;
    cinsiyetNo?: string;
    yasi?: number;
    eposta?: string;
    telefonNumarasi?: string;
    profilFotoUrl?: string;
    yaratilmaTarihi?: Date;
    sonAktifOlma?: Date;
    fotograflari?: Foto[]
}


export class KullaniciYaz {
    id?: number;
    kullaniciAdi?: string;
    unvan?: string;
    ad?: string;
    digerAd?: string;
    soyad?: string;
    cinsiyetNo?: string;
    dogumTarihi: Date;
    eposta?: string;
    epostaOnaylandi?: boolean;
    telefonNumarasi?: string;
    telefonOnaylandi?: boolean;
    aktif?: boolean;
    profilFotoUrl?: string;
    yaratilmaTarihi?: Date;
    sonAktifOlma?: Date;
    tamAdi?:string;
    fotograflari?: Foto[]
}
export class KullanicilarVeriSeti {
    kullaniciSonuc:ListeSonuc<Kullanici>;
}
export class KullaniciVeriSeti {
  kullaniciSonuc:KayitSonuc<KullaniciYaz>;
  cinsiyetler: Cinsiyet[];
}
