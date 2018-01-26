import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { environment } from '../../environments/environment';
import { BadInputError } from '../_hatalar/bad-input';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotFoundError } from '../_hatalar/not-found-error';
import { AppError } from '../_hatalar/app-error';
import { ListeSonuc, KayitSonuc } from '../_models/sonuc';
import { HttpHeaders } from '@angular/common/http';


@Injectable()
export class DataService {

    private serverUrl = environment.apiUrl;
    private address: string;
    constructor(private mapUrl: string, private http: HttpClient) {
        this.address = `${this.serverUrl}/${mapUrl}`;
    }
    private adresBos(adres: string): boolean {
        if (adres === '' || adres === null) {
            return true;
        } else {
            return false;
        }
    }
    private adresiBelirle(adres: string): string {
        let url = '';
        if (this.adresBos(adres)) {
            url = this.address;
        } else {
            url = `${this.serverUrl}/${adres}`;
        }
        return url;
    }
    list<T>(mapUrl: string = ''): Observable<T> {
        const url = this.adresiBelirle(mapUrl);
        return this.http.get<T>(url)
            .catch(this.handleError);
    }
    get<T>(id = 0, mapUrl: string = ''): Observable<T> {
        let url = this.adresiBelirle(mapUrl);
        if (id > 0) {
            url += '/' + id;
        }
        return this.http.get<T>(url)
            .catch(this.handleError);
    }
    create<T>(resource?: any, mapUrl: string = ''): Observable<T> {
        const url = this.adresiBelirle(mapUrl);
        return this.http.post<T>(this.address, JSON.stringify(resource))
            .catch(this.handleError);
    }

    update<T>(id, resource, mapUrl: string = ''): Observable<T> {
        const url = this.adresiBelirle(mapUrl);
        return this.http.put<T>(this.address + '/' + resource.id, JSON.stringify(resource))
            .catch(this.handleError);
    }
    patch<T>(id, resource, mapUrl: string = ''): Observable<T> {
        const url = this.adresiBelirle(mapUrl);
        return this.http.patch<T>(this.address + '/' + resource.id, JSON.stringify(resource))
            .catch(this.handleError);
    }
    delete<T>(id?: any, mapUrl: string = ''): Observable<T> {
        let url = this.adresiBelirle(mapUrl);
        if (id != null && +id > 0) {
            url += '/' + id;
        }
        return this.http.delete<T>(this.address)
            .catch(this.handleError);
    }
    handleError(error: Response) {
        if (error.status === 400) {
            return Observable.throw(new BadInputError(error.json()));
        }
        if (error.status === 404) {
            return Observable.throw(new NotFoundError());
        }
        return Observable.throw(new AppError(error));
    }

}