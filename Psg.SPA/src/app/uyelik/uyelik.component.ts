import { Kullanici } from './../_models/kullanici';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { timeout } from 'q';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/bs-moment';
import { tr } from 'ngx-bootstrap/locale';


@Component({
  selector: 'app-uyelik',
  templateUrl: './uyelik.component.html',
  styleUrls: ['./uyelik.component.css'],

})
export class UyelikComponent implements OnInit {

  @Output() iptal = new EventEmitter();
  kullanici: Kullanici
  bsConfig: Partial<BsDatepickerConfig>;
  uyelikFormu: FormGroup;
  uyelikBasvurusuTamam:boolean;

  constructor(
    private authService: AuthService,
    private uyarici: AlertifyService,
    private fb: FormBuilder,
    private _localeService: BsLocaleService) { }

  uyeliKFromunuYarat() {
    this.uyelikFormu = this.fb.group({
      kullaniciAdi: ['', Validators.required],
      sifre: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(10)]],
      sifreKontrol: ['',],
      unvan: ['', [Validators.minLength(2), Validators.maxLength(10)]],
      ad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      digerAd: ['', [Validators.minLength(2), Validators.maxLength(50)]],

      soyad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      cinsiyetNo: [1],
      dogumTarihi: [null, Validators.required],
      ePosta: ['', [Validators.required, Validators.email]],
      telefonNumarasi: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(10)]],
    },
      { validator: this.sifreKontrolValidator }
    );
  }

  sifreKontrolValidator(g: FormGroup) {
    return g.get('sifre').value === g.get('sifreKontrol').value ? null : { 'sifreKontrolBasarisiz': true };
  }

  ngOnInit() {
    defineLocale('tr', tr);
    this._localeService.use('tr');

    this.bsConfig = { containerClass: 'theme-red' }
    this.kullaniciVarsaCikisaZorla();
    this.uyeliKFromunuYarat();

  }
  private kullaniciVarsaCikisaZorla() {
    if (this.authService.loggedIn())
      this.authService.logout();
  }
  uyeol() {
    // if (this.uyelikFormu.valid) {
      this.kullanici = Object.assign({}, this.uyelikFormu.value);
     this.authService.register(this.kullanici).subscribe(()=> {
       this.uyarici.success("Üyelik başarılı!");
       this.uyarici.warning("Adresinize bir eposta gönderildi.");
       this.uyarici.warning("Lütfen gelen kutunuza bakın ve hesabınızı aktifleştirin.");
       this.uyelikBasvurusuTamam=true;
     },
    hata=>this.uyarici.error("Üyelik işleminiz gerçekleşmedi!!"));
    // }
    // this.authService.register(this.model).subscribe(() =>
    //   this.uyarici.success('Üyelik başarılı.'),
    //   error => this.uyarici.error(error));
    // console.log(this.uyelikFormu.value);
  }
  vazgec() {
    this.iptal.emit(false);
    this.uyarici.message('Üyelik isteği iptal edildi...');
  }

  kapat(){
    this.uyelikBasvurusuTamam=false;
    this.iptal.emit(false);
  }
}
