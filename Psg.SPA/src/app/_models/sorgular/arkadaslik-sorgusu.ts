import { SorguBase } from './sorgu-base';


export class ArkadaslikSorgusu extends SorguBase {
    teklifEdilenler?: boolean;
    teklifEdenler?: boolean;
    kabulEdilenler?: boolean;
    cevapBeklenenler?: boolean;
    cevaplananlar?: boolean;
    kullaniciNo?: boolean;
}
