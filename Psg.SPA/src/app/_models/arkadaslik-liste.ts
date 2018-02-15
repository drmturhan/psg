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
