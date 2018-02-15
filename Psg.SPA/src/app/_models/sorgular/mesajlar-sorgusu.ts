import { SorguBase } from './sorgu-base';

export class MesajlasmaSorgusu extends SorguBase {

    kullaniciNo?: number;
    digerKullaniciNo?: number;
    gelenMesajlar?: boolean;
    gidenMesajlar?: boolean;
    okunmamisMesajlar?: boolean;
}
