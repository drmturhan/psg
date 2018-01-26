import { ArkadaslarimResolver } from './_resolvers/kullanici/arkadaslarim-resolver';
import { ArkadaslarimComponent } from './kullanicilar/arkadaslarim/arkadaslarim.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { KullaniciService } from './_services/kullanici.service';
import { AlertifyService } from './_services/alertify.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router/';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { UyelikComponent } from './uyelik/uyelik.component';
import { JwtModule } from '@auth0/angular-jwt';
import { RequestOptions, Http } from '@angular/http/';
import { AuthService } from './_services/auth.service';
import { MesajlarComponent } from './mesajlar/mesajlar.component';
import { NgxGalleryModule } from 'ngx-gallery';
import { appRoot } from './route';
import { AuthGuard } from './_guards/auth.guard';
import { RandevuComponent } from './randevu/randevu.component';
import { YukleComponent } from './yukle/yukle.component';
import { BulComponent } from './bul/bul.component';
import { KullaniciListesiComponent } from './kullanicilar/kullanici-listesi/kullanici-listesi.component';
import { HttpClientModule } from '@angular/common/http';
import { KullaniciKartiComponent } from './kullanicilar/kullanici-karti/kullanici-karti.component';
import { KullaniciDetayComponent } from './kullanicilar/kullanici-detay/kullanici-detay.component';
import { TabsModule, BsDropdownModule, TooltipModule } from 'ngx-bootstrap';
import { KullaniciDetayResolver } from './_resolvers/kullanici/kullanici-detay-resolver';
import { KullaniciDuzeltComponent } from './kullanicilar/kullanici-duzelt/kullanici-duzelt.component';
import { VeriYuklenemediComponent } from './ortak/components/veri-yuklenemedi/veri-yuklenemedi.component';
import { KullaniciListesiResolver } from './_resolvers/kullanici/kullanici-listesi-resolver.';
import { ProfilimResolver } from './_resolvers/kullanici/profilim-resolver';
import { KullanicidakiDegisikliklerKaybolsunmuGuard } from './_guards/kullanici/kullanicidaki-degisiklikler-kaybolsunmu.service';
import { NgModule, LOCALE_ID, ErrorHandler } from '@angular/core';
import { FotografDuzenleyiciComponent } from './ortak/components/fotograf-duzenleyici/fotograf-duzenleyici.component';
import { FileUploadModule } from 'ng2-file-upload';
import { UyelikBasariliComponent } from './uyelik/akis/uyelik-basarili/uyelik-basarili.component';
import { registerLocaleData } from '@angular/common';
import localeTr from '@angular/common/locales/tr';
import localeTrExtra from '@angular/common/locales/extra/tr';
import { TimeAgoPipe } from 'time-ago-pipe';
import { KullaniciAsyncValidators } from './uyelik/kullanici-adi-var-validator.service';
import { AppError } from './_hatalar/app-error';
import { MTAppErrorHandler } from './_hatalar/app-error-handler';
import { CinsiyetlerService } from './_services/cinsiyetler.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './_services/auth-interceptor';



registerLocaleData(localeTr, 'tr-TR', localeTrExtra);
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    AnasayfaComponent,
    UyelikComponent,
    MesajlarComponent,
    RandevuComponent,
    YukleComponent,
    BulComponent,
    KullaniciListesiComponent,
    KullaniciKartiComponent,
    KullaniciDetayComponent,
    VeriYuklenemediComponent,
    KullaniciDuzeltComponent,
    FotografDuzenleyiciComponent,
    UyelikBasariliComponent,
    ArkadaslarimComponent,
    TimeAgoPipe
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoot),
    TabsModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    HttpModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
        whitelistedDomains: ['http://localhost:4200'],
        skipWhenExpired: true,
      }
    }),
    FormsModule,
    ReactiveFormsModule,
    NgxGalleryModule,
    FileUploadModule

  ],


  providers: [
    {
      provide: LOCALE_ID,
      useValue: 'tr-TR'
    },
    {
      provide: ErrorHandler,
      useClass: MTAppErrorHandler
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthService,
    KullaniciService,
    CinsiyetlerService,
    AlertifyService,
    AuthGuard,
    KullaniciDetayResolver,
    KullaniciListesiResolver,
    ProfilimResolver,
    ArkadaslarimResolver,
    KullanicidakiDegisikliklerKaybolsunmuGuard,
    KullaniciAsyncValidators
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
