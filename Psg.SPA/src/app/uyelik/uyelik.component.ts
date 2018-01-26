import { Response } from '@angular/http';
import { UyeBilgisi } from './../_models/kullanici';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, QueryList, ViewChildren, ElementRef } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { FormGroup, FormControl, FormBuilder, Validators, FormControlName, AbstractControl } from '@angular/forms';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/bs-moment';
import { tr } from 'ngx-bootstrap/locale';
import { Cinsiyet } from '../_models/foto';
import { KullaniciService } from '../_services/kullanici.service';
import { GenericValidator } from '../_validators/generic-validator';
import { validasyonMesajlari, uyelikFormuEksikMesaji } from './validasyon.mesajlari';
import { ViewChild } from '@angular/core';
import { UyelikValidatorleri } from './uyelik.validators';
import { AppError } from '../_hatalar/app-error';
import { Observable } from 'rxjs/Observable';
import { BadInputError } from '../_hatalar/bad-input';
import { CinsiyetlerService } from '../_services/cinsiyetler.service';


@Component({
  selector: 'app-uyelik',
  templateUrl: './uyelik.component.html',
  styleUrls: ['./uyelik.component.css']
})
export class UyelikComponent implements OnInit, AfterViewInit {

  @Output() iptal = new EventEmitter();

  @ViewChildren(FormControlName, { read: ElementRef })
  formInputElements: QueryList<any>;
  bsConfig: Partial<BsDatepickerConfig>;
  uyelikFormu: FormGroup;
  uyelikBasvurusuTamam: boolean;
  cinsiyetler: Cinsiyet[];
  hatalar: string[] = [];
  serverValidasyon: any = {};
  public errorMessage: string;
  public validationMessages: any = {};
  public displayMessage: any = {};
  public genericValidator: GenericValidator;
  uyeOlAktif = false;
  uyeOlTooltipMesaj = 'Form bilgileri eksik. Bu bilgilerle üye olamazsınız';
  constructor(
    private authService: AuthService,
    private kullaniciService: KullaniciService,
    private cinstiyetlerService: CinsiyetlerService,
    private uyarici: AlertifyService,
    private fb: FormBuilder,
    private _localeService: BsLocaleService) {
    this.validationMessages = validasyonMesajlari();
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  uyeliKFromunuYarat() {

    this.uyelikFormu = this.fb.group({
      kullaniciAdi: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        UyelikValidatorleri.boslukIceremez
      ], this.isUserNameUnique.bind(this)],
      sifreGrup: this.fb.group(
        {
          sifre: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(18)]],
          sifreKontrol: ['', [Validators.required]]
        },
        { validator: UyelikValidatorleri.sifreKontrol }
      ),
      unvan: ['', [Validators.minLength(2), Validators.maxLength(10)]],
      ad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      digerAd: ['', [Validators.minLength(2), Validators.maxLength(50)]],

      soyad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      cinsiyetNo: [1],
      dogumTarihi: [null, Validators.required],
      ePosta: ['', [Validators.required, Validators.email]],
      telefonNumarasi: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(10)]],
    });
  }

  isUserNameUnique(control: FormControl) {
    const q = new Promise((resolve, reject) => {
      setTimeout(() => {
        this.kullaniciService.kullaniciVar(control.value).subscribe(sonuc => {
          if (sonuc) {
            resolve({ 'kullaniciAdiKullaniliyor': true });
          } else {
            resolve(null);
          }
        }, () => { resolve({ 'kullaniciAdiKullaniliyor': true }); });
      }, 500);
    });
    return q;
  }

  ngOnInit() {
    defineLocale('tr', tr);
    this._localeService.use('tr');
    this.bsConfig = { containerClass: 'theme-red' };
    this.kullaniciVarsaCikisaZorla();
    this.cinstiyetlerService.list<Cinsiyet[]>().subscribe(
      data => this.cinsiyetler = data);
    this.uyeliKFromunuYarat();

  }

  public ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs = this.formInputElements.map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    Observable
      .merge(this.uyelikFormu.valueChanges, ...controlBlurs)
      .debounceTime(800)
      .subscribe(value => {
        this.displayMessage = this.genericValidator.processMessages(this.uyelikFormu);
        this.uyeOlAktif = true;
      });
    // const kullaniciAdiControl = this.uyelikFormu.get('kullaniciAdi');
    // if (kullaniciAdiControl) {
    //   kullaniciAdiControl.valueChanges
    //     .filter(val => val.length >= 3).debounceTime(800)
    //     .switchMap(val =>
    //       this.kullaniciService.kullaniciVar(val)).subscribe(sonuc => {
    //         if (sonuc) {
    //           kullaniciAdiControl.setErrors({ 'kullaniciVar': true });
    //         } else {
    //           kullaniciAdiControl.setErrors(null);
    //         }
    //       });
    // }
  }


  private kullaniciVarsaCikisaZorla() {
    if (this.authService.loggedIn()) {
      this.authService.logout();
    }
  }
  uyeol() {
    // if (this.uyelikFormu.valid) {
    const gonderilecekBilgi: UyeBilgisi = Object.assign({}, this.uyelikFormu.value);
    gonderilecekBilgi.sifre = this.uyelikFormu.get('sifreGrup.sifre').value;
    this.authService.register(gonderilecekBilgi).subscribe(() => {
      this.uyarici.success('Üyelik başarılı!');
      this.uyarici.warning('Adresinize bir eposta gönderildi.');
      this.uyarici.warning('Lütfen gelen kutunuza bakın ve hesabınızı aktifleştirin.');
      this.uyelikBasvurusuTamam = true;
    },
      (hata: AppError) => {
        if (hata instanceof BadInputError) {
          this.displayMessage = hata.orjinalHata;
          this.uyelikFormu.setErrors(this.displayMessage);
        } else {
          throw hata;
        }
      });
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

  kapat() {
    this.uyelikBasvurusuTamam = false;
    this.iptal.emit(false);
  }

  validasyonuSifirla() {
    this.serverValidasyon = {};
  }
}
