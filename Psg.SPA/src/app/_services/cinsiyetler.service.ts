import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';
import { ListeSonuc} from '../_models/sonuc';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';
import { Cinsiyet } from '../_models/foto';


@Injectable()
export class CinsiyetlerService {
    private url: string;
    constructor(private dataService: DataService) {
        this.url = environment.apiUrl;
    }
    list() {
        return this.dataService.get<Cinsiyet[]>(`${this.url}cinsiyetler`);
    }
}