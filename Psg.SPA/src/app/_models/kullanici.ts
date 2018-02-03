import { KisiFoto, Cinsiyet } from './foto';
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
    fotograflari?: KisiFoto[];
}


export class KullaniciBilgi {
    id?: number;
    tamAdi?: string;
    CinsiyetNo?: number;
    yasi?: number;
    eposta?: string;
    epostaOnaylandi?: boolean;
    telefonNumarasi?: string;
    telefonNumarasiOnaylandi?: boolean;
    profilFotoUrl?: string;
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
    tamAdi?: string;
    fotograflari?: KisiFoto[];
}
export class KullanicilarVeriSeti {
    kullaniciSonuc: ListeSonuc<Kullanici>;
}
export class KullaniciVeriSeti {
    kullaniciSonuc: KayitSonuc<KullaniciYaz>;
    cinsiyetler: Cinsiyet[];
}

export class UyeBilgisi {

    constructor() {
        this.sifre = '';
    }
    kullaniciAdi?: string;
    sifre?: string;
    unvan?: string;
    ad?: string;
    digerAd?: string;
    soyad?: string;
    cinsiyetNo?: string;
    dogumTarihi: Date;
    eposta?: string;
    telefonNumarasi?: string;
}


