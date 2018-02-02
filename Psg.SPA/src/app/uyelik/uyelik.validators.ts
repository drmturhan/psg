import { AbstractControl, ValidationErrors, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { KullaniciService } from '../_services/kullanici.service';
import 'rxjs/add/operator/map';
import { Injectable, Inject, Injector } from '@angular/core';

@Injectable()
export class UyelikValidatorleri {
    constructor(private kullaniciService: KullaniciService) {
    }
    boslukIceremez(control: AbstractControl): ValidationErrors | null {
        if ((control.value as string).indexOf(' ') >= 0) {
            return { boslukIceremez: true };
        }
        return null;
    }
    sifreKontrol(control: AbstractControl): ValidationErrors | null {
        const sifre = control.get('sifre');
        const sifreKontrol = control.get('sifreKontrol');
        if (sifre.pristine || sifreKontrol.pristine) {
            return null;
        }
        if (!sifre.valid) {
            return null;
        }
        if (sifreKontrol.value === null) {
            return null;
        }
        if (sifre.value === sifreKontrol.value) {
            return null;
        }
        return { 'sifreKontrolBasarisiz': true };
    }

    isUserNameUnique(control: FormControl) {

        const q = new Promise((resolve, reject) => {
            setTimeout(() => {
                this.kullaniciService.kullaniciAdiKullanimda(control.value).subscribe(sonuc => {
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
    isMailUnique(control: FormControl) {

        const q = new Promise((resolve, reject) => {
            setTimeout(() => {
                this.kullaniciService.mailAdresiKullanimda(control.value).subscribe(sonuc => {
                    if (sonuc) {
                        resolve({ 'epostaKullaniliyor': true });
                    } else {
                        resolve(null);
                    }
                }, () => { resolve({ 'epostaKullaniliyor': true }); });
            }, 1000);
        });
        return q;
    }
}