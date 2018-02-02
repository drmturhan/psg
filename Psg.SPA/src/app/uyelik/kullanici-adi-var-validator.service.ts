
import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, ValidatorFn, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { KullaniciService } from '../_services/kullanici.service';
import { FormControl } from '@angular/forms';


function isEmptyInputValue(value: any): boolean {
    // we don't check for string here so it also works with arrays
    return value == null || value.length === 0;
}

@Injectable()
export class KullaniciAsyncValidators {
    constructor(private kullaniciService: KullaniciService) {
    }
    kullaniciAdiVar(control: FormControl): Promise<any> | Observable<any> {
        const promise = new Promise<any>((resolve, reject) => {
            if (control.value == null || control.value === '') {
                resolve(null);
            } else {
                this.kullaniciService.kullaniciAdiVar(control.value).map(sonuc => {
                    sonuc ? resolve({ 'kullaniciVar': true }) : resolve(null);
                });
            }
        });
        return promise;
    }
}