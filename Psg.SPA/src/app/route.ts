import { ListelerComponent } from './listeler/listeler.component';
import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { Routes } from '@angular/router';
import { UyeListesiComponent } from './uye-listesi/uye-listesi.component';
import { MesajlarComponent } from './mesajlar/mesajlar.component';
import { AuthGuard } from './_guards/auth.guard';
export const appRoot: Routes = [
    { path: 'anasayfa', component: AnasayfaComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [{ path: 'uyeler', component: UyeListesiComponent},
        { path: 'mesajlar', component: MesajlarComponent },
        { path: 'listeler', component: ListelerComponent },]
    },

    { path: '**', redirectTo: 'anasayfa', pathMatch: 'full' }
]