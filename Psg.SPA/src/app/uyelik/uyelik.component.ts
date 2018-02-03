import {
  AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, QueryList, ViewChild,
  ViewChildren
} from '@angular/core';
import {
  AbstractControl, FormBuilder, FormControl, FormControlName, FormGroup, Validators
} from '@angular/forms';

import { Observable } from 'rxjs/Observable';
import { AppError } from '../_hatalar/app-error';
import { BadInputError } from '../_hatalar/bad-input';
import { Cinsiyet } from '../_models/foto';
import { UyeBilgisi } from '../_models/kullanici';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { CinsiyetlerService } from '../_services/cinsiyetler.service';
import { KullaniciService } from '../_services/kullanici.service';
import { GenericValidator } from '../_validators/generic-validator';
import { UyelikValidatorleri } from './uyelik.validators';
import { uyelikFormuEksikMesaji, validasyonMesajlari } from './validasyon.mesajlari';
import { BsDatepickerConfig } from 'ngx-bootstrap';

@Component({
  selector: 'app-uyelik',
  templateUrl: './uyelik.component.html',
  styleUrls: ['./uyelik.component.css'],
  providers: [UyelikValidatorleri]
})
export class UyelikComponent implements OnInit, AfterViewInit {

  @Output() iptal = new EventEmitter();
  bsConfig: Partial<BsDatepickerConfig>;
  @ViewChildren(FormControlName, { read: ElementRef })
  formInputElements: QueryList<any>;
  uyelikFormu: FormGroup;
  uyelikBasvurusuTamam: boolean;
  cinsiyetler: Cinsiyet[];
  hatalar: string[] = [];
  serverValidasyon: any = {};
  public errorMessage: string;
  public validationMessages: any = {};
  public displayMessage: any = {};
  public genericValidator: GenericValidator;
  public mouseUyeolUstundaValidasyonGosterilsin = false;
  uyeOlAktif = false;
  uyeOlTooltipMesaj = 'Form bilgileri eksik. Bu bilgilerle üye olamazsınız';
  constructor(
    private authService: AuthService,
    private kullaniciService: KullaniciService,
    private cinstiyetlerService: CinsiyetlerService,
    private uyarici: AlertifyService,
    private uyelikValidatorlari: UyelikValidatorleri,
    private fb: FormBuilder) {
    this.validationMessages = validasyonMesajlari();
    this.genericValidator = new GenericValidator(this.validationMessages);

  }

  uyeliKFromunuYarat() {

    this.uyelikFormu = this.fb.group({
      kullaniciAdi: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
        this.uyelikValidatorlari.boslukIceremez
      ], this.uyelikValidatorlari.isUserNameUnique.bind(this)],
      sifreGrup: this.fb.group(
        {
          sifre: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(18)]],
          sifreKontrol: ['', [Validators.required]]
        },
        { validator: this.uyelikValidatorlari.sifreKontrol }
      ),
      unvan: ['', [Validators.minLength(2), Validators.maxLength(10)]],
      ad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      digerAd: ['', [Validators.minLength(2), Validators.maxLength(50)]],

      soyad: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      cinsiyetNo: [1],
      dogumTarihi: ['', Validators.required],
      ePosta: ['', [Validators.required, Validators.email], [this.uyelikValidatorlari.isMailUnique.bind(this)]],
      telefonNumarasi: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(10)]],
    });
  }
  validasyonuBilgisiGorunsun(form) {
    this.mouseUyeolUstundaValidasyonGosterilsin = true && !this.uyelikFormu.valid;
    if (this.mouseUyeolUstundaValidasyonGosterilsin === true) {
      this.displayMessage = this.genericValidator.processMessages(this.uyelikFormu, true);
    }
  }
  validasyonBilgisiGizlensin(form) {
    this.mouseUyeolUstundaValidasyonGosterilsin = false;
  }
  formValiasyonAktiflestir() {

  }
  denemeKullaniciYarat() {
    this.uyelikFormu.patchValue({
      kullaniciAdi: 'ozge',
      sifreGrup: {
        sifre: 'Akd34630.',
        sifreKontrol: 'Akd34630.'
      },
      unvan: 'DoProf.Dr.',
      ad: 'Özge',
      soyad: 'Turhan',
      digerAd: '',
      cinsiyetNo: 2,
      ePosta: 'drmuratturhan@gmail.com',
      telefonNumarasi: 5332737353,
      dogumTarihi: new Date('1974-1-16')

    });
  }



  ngOnInit() {
    this.bsConfig = { containerClass: 'theme-red', dateInputFormat: 'DD.MM.YYYY' };
    this.kullaniciVarsaCikisaZorla();
    this.cinstiyetlerService.list().subscribe(
      data => this.cinsiyetler = data);
    this.uyeliKFromunuYarat();
    this.denemeKullaniciYarat();
  }

  public ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    const controlBlurs = this.formInputElements.map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    Observable
      .merge(this.uyelikFormu.valueChanges, ...controlBlurs)
      .debounceTime(600)
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
    if (this.uyelikFormu.valid === false) {
      this.displayMessage = this.genericValidator.processMessages(this.uyelikFormu, true);
      this.uyarici.error('Form bilgileri hatalı!');
      return;
    }
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