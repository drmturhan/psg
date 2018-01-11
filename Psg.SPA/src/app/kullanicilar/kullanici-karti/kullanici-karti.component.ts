import { Kullanici } from './../../_models/kullanici';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'mt-kullanici-karti',
  templateUrl: './kullanici-karti.component.html',
  styleUrls: ['./kullanici-karti.component.css']
})
export class KullaniciKartiComponent implements OnInit {
@Input() kullanici:Kullanici
  constructor() { }

  ngOnInit() {

  }

}
