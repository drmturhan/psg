import { SorguBase } from './sorgu-base';
import { AdDeger } from '../arkadaslik-liste';

export class ArkadaslikSorgusu extends SorguBase {
    teklifEdilenler?: boolean;
    teklifEdenler?: boolean;
    kabulEdilenler?: boolean;
    cevapBeklenenler?: boolean;
    cevaplananlar?: boolean;
    kullaniciNo?: boolean;
}
