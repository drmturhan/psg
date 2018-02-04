import { SorguBase } from './sorgu-base';
import { AdDeger } from '../arkadaslik-liste';

export class ArkadaslikSorgusu extends SorguBase {
    teklifEdenKullaniciNo?: number;
    cevapVerecekKullaniciNo?: number;
    filtreTipi?: AdDeger;
}
