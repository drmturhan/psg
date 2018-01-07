import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { AnasayfaComponent } from './anasayfa/anasayfa.component';
import { UyelikComponent } from './uyelik/uyelik.component';



@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    AnasayfaComponent,
    UyelikComponent
],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
