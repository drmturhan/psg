import { Foto } from './foto';
export class Kullanici {
    id?: number;
    kullaniciAdi?: string;
    tamAdi?: string;
    cinsiyeti?: string;
    yasi?: number;
    eposta?: string;
    telefonNumarasi?: string;
    asilFotoUrl?: string;
    yaratilmaTarihi?: Date;
    sonaktifOlma?: Date;
    fotograflari?: Foto[]
}


export class KullaniciYaz {
    id?: number;
    kullaniciAdi?: string;
    unvan?: string;
    ad?: string;
    digerAd?:string;
    soyad?: string;
    cinsiyeti?: string;
    dogumTarihi:Date;
    eposta?: string;
    telefonNumarasi?: string;
    aktif?:boolean;
    asilFotoUrl?: string;
    yaratilmaTarihi?: Date;
    sonaktifOlma?: Date;
    fotograflari?: Foto[]
}