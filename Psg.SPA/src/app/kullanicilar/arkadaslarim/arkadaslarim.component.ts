import { ListeSonuc } from './../../_models/sonuc';
import { ArkadaslikListe } from './../../_models/arkadaslik-liste';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-arkadaslarim',
  templateUrl: './arkadaslarim.component.html',
  styleUrls: ['./arkadaslarim.component.css']
})
export class ArkadaslarimComponent implements OnInit {

  aramaParametreleri: any = {};

  constructor(private route: ActivatedRoute) { }
  arkadasliklarim: ListeSonuc<ArkadaslikListe> = new ListeSonuc<ArkadaslikListe>();
  ngOnInit() {
    this.route.data.subscribe((data: ListeSonuc<ArkadaslikListe>) => {

      let kullaniciVeriSeti = data['arkadaslarim'];
      if (kullaniciVeriSeti && kullaniciVeriSeti.basarili) {
        this.arkadasliklarim = kullaniciVeriSeti;
      }
    });
  }
  yukle() { };
}
