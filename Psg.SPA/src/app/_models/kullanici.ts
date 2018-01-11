import { Foto } from './foto';
export class Kullanici {
    id?:number;
    kullaniciAdi?:string;
    tamAdi?:string;
    cinsiteti?:string;
    yasi?:number;
    eposta?:string;
    telefonNumarasi?:string;
    asilFotoUrl?:string;
    yaratilmaTarihi?:Date;
    sonaktifOlma?:Date;
    fotograflari?:Foto[]
}
