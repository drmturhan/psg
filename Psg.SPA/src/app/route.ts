import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { Routes } from '@angular/router';
import { UyeListesiComponent } from './uye-listesi/uye-listesi.component';
import { MesajlarComponent } from './mesajlar/mesajlar.component';
import { AuthGuard } from './_guards/auth.guard';
import { BulComponent } from './bul/bul.component';
import { YukleComponent } from './yukle/yukle.component';
import { RandevuComponent } from './randevu/randevu.component';
export const appRoot: Routes = [
    { path: 'anasayfa', component: AnasayfaComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [{ path: 'uyeler', component: UyeListesiComponent },
        { path: 'mesajlar', component: MesajlarComponent },
        { path: 'uyelistesi', component: UyeListesiComponent },
        { path: 'bul', component: BulComponent },
        { path: 'yukle', component: YukleComponent },
        { path: 'randevu', component: RandevuComponent },]
    },

    { path: '**', redirectTo: 'anasayfa', pathMatch: 'full' }
]