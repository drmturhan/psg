import { ArkadaslikListe } from './../../_models/arkadaslik-liste';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-arkadaslarim',
  templateUrl: './arkadaslarim.component.html',
  styleUrls: ['./arkadaslarim.component.css']
})
export class ArkadaslarimComponent implements OnInit {

  aramaParametreleri:any={};

  constructor(private route: ActivatedRoute) { }
  arkadasliklarim:ArkadaslikListe [];
  ngOnInit() {
    this.route.data.subscribe(data => {
      let kullaniciVeriSeti = data['arkadaslarim'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.basarili) {
        this.arkadasliklarim = kullaniciVeriSeti.donenListe;
      }
    });
  }
  yukle(){};
}
