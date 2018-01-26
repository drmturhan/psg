import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { KullaniciService } from '../_services/kullanici.service';
import 'rxjs/add/operator/map';
import { Injectable } from '@angular/core';

@Injectable()
export class UyelikValidatorleri {


    static boslukIceremez(control: AbstractControl): ValidationErrors | null {
        if ((control.value as string).indexOf(' ') >= 0) {
            return { boslukIceremez: true };
        }
        return null;
    }
    static sifreKontrol(control: AbstractControl): ValidationErrors | null {
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

    static kullaniciAdiVar(control: AbstractControl, kullaniciService: KullaniciService): Promise<ValidationErrors | null> {
        return new Promise((resolve, reject) => {
            let kullaniciVar = false;
            kullaniciService.kullaniciVar(control.value).map(res => {
                kullaniciVar = res;
                if (kullaniciVar) {
                    resolve({ 'kullaniciVar': true });
                } else {
                    resolve(null);
                }
            });
        });

    }
}