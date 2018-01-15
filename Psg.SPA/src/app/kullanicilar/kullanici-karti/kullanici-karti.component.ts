import { Kullanici } from './../../_models/kullanici';
import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'mt-kullanici-karti',
  templateUrl: './kullanici-karti.component.html',
  styleUrls: ['./kullanici-karti.component.css']
})
export class KullaniciKartiComponent implements OnInit {
  @Input() kullanici: Kullanici
  bosFotoUrl: string = environment.bosFotoUrl;
  constructor(public authService: AuthService) { }
  ngOnInit() {

  }

}
