import { Kullanici } from './kullanici';
export class ArkadasliklarimListe {
    teklifEden?: Arkadas;
    teklifEdilen?: Arkadas;
    istekTarihi?: Date;
    cevapTarihi?: Date;
    karar?: boolean;
}
export class Arkadas {
    id?: number;
    tamAdi?: string;
    cinsiyetAdi?: string;
    yasi?: number;
    eposta?: string;
    epostaOnaylandi?: string;
    telefonNumarasi?: string;
    telefonOnaylandi?: string;
    profilFotoUrl?: string;
}

export class AdDeger {
    deger: number;
    ad: string;
}

const arkadaslikListeTipleri: AdDeger[] = [
    { deger: 0, ad: 'Tümü' },
    { deger: 1, ad: 'Adece kabul edilenler' },
    { deger: 2, ad: 'Sadece reddedilenler' },
    { deger: 3, ad: 'Sadece cevap beklenenler' },
    { deger: 4, ad: 'Sadece cevap verilenler' },
];
