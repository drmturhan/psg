import { ActivatedRouteSnapshot } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-veri-yuklenemedi',
  templateUrl: './veri-yuklenemedi.component.html',
  styleUrls: ['./veri-yuklenemedi.component.css']
})
export class VeriYuklenemediComponent implements OnInit {

  hataMesajlari:string[]
  constructor(private route:ActivatedRouteSnapshot) { }

  ngOnInit() {
    const mesajlar=this.route.params['mesajlar'];
    if (mesajlar)
    this.hataMesajlari=mesajlar;
  }

}
