import { ArkadaslarimResolver } from './_resolvers/kullanici/arkadaslarim-resolver';
import { ArkadaslarimComponent } from './uyelik/arkadaslarim/arkadaslarim.component';

import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { Routes } from '@angular/router';
import { MesajlarComponent } from './mesajlar/mesajlar.component';
import { AuthGuard } from './_guards/auth.guard';
import { BulComponent } from './bul/bul.component';
import { YukleComponent } from './yukle/yukle.component';
import { RandevuComponent } from './randevu/randevu.component';
import { KullaniciListesiComponent } from './kullanicilar/kullanici-listesi/kullanici-listesi.component';
import { KullaniciDetayComponent } from './kullanicilar/kullanici-detay/kullanici-detay.component';
import { KullaniciDetayResolver } from './_resolvers/kullanici/kullanici-detay-resolver';
import { KullaniciDuzeltComponent } from './kullanicilar/kullanici-duzelt/kullanici-duzelt.component';
import { KullaniciListesiResolver } from './_resolvers/kullanici/kullanici-listesi-resolver.';
import { VeriYuklenemediComponent } from './ortak/components/veri-yuklenemedi/veri-yuklenemedi.component';
import { ProfilimResolver } from './_resolvers/kullanici/profilim-resolver';
import { KullanicidakiDegisikliklerKaybolsunmuGuard } from './_guards/kullanici/kullanicidaki-degisiklikler-kaybolsunmu.service';

import { UyelikBasariliComponent } from './uyelik/akis/uyelik-basarili/uyelik-basarili.component';
import { YenidenAktiflestirComponent } from './uyelik/akis/yeniden-aktiflestir/yeniden-aktiflestir.component';
import { EpostaOnaylandiComponent } from './uyelik/akis/eposta-onay/eposta-onaylandi.component';
import { ProfilimComponent } from './uyelik/profilim/profilim.component';
import { KullaniciMesajlariResolver } from './_resolvers/kullanici/kullanici-mesajlari-resolver';
export const appRoot: Routes = [
    { path: '', redirectTo: 'anasayfa', pathMatch: 'full' },
    { path: 'yuklemeHatasi', component: VeriYuklenemediComponent },
    { path: 'epostaonaylandi', component: EpostaOnaylandiComponent },
    { path: 'onaypostasigonder', component: YenidenAktiflestirComponent },
    { path: 'uyelikbasarili', component: UyelikBasariliComponent },
    { path: 'anasayfa', component: AnasayfaComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'kullanicilar', component: KullaniciListesiComponent, resolve: { kullanicilar: KullaniciListesiResolver } },
            { path: 'kullanicilar/:id', component: KullaniciDetayComponent, resolve: { kullanici: KullaniciDetayResolver } },
            {
                path: 'profilim', component: ProfilimComponent,
                canDeactivate: [KullanicidakiDegisikliklerKaybolsunmuGuard], resolve: { kullaniciVeriSeti: ProfilimResolver }
            },
            { path: 'arkadasliklarim', component: ArkadaslarimComponent, resolve: { arkadaslarim: ArkadaslarimResolver } },
            { path: 'mesajlarim', component: MesajlarComponent, resolve: { mesajlarim: KullaniciMesajlariResolver } },
            { path: 'bul', component: BulComponent },
            { path: 'yukle', component: YukleComponent },
            { path: 'randevu', component: RandevuComponent }
        ]
    },
    { path: '**', redirectTo: 'anasayfa', pathMatch: 'full' }
];