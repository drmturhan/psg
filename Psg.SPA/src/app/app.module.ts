import { AlertifyService } from './_services/alertify.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router/';
import { HttpClientModule } from '@angular/common/http'
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { UyelikComponent } from './uyelik/uyelik.component';
import { JwtModule } from '@auth0/angular-jwt';
import { RequestOptions, Http } from '@angular/http/';
import { AuthService } from './_services/auth.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UyeListesiComponent } from './uye-listesi/uye-listesi.component';
import { MesajlarComponent } from './mesajlar/mesajlar.component';
import { appRoot } from './route';
import { ListelerComponent } from './listeler/listeler.component';
import { AuthGuard } from './_guards/auth.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    AnasayfaComponent,
    UyelikComponent,
    UyeListesiComponent,
    MesajlarComponent,
    ListelerComponent
],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoot),
    NgbModule.forRoot(),
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
        whitelistedDomains: ['localhost:4200'],
        skipWhenExpired: true,
      }
    }),
    FormsModule,
  ],
  

  providers: [
    AuthService,
    AlertifyService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
