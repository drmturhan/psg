import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-anasayfa',
  templateUrl: './anasayfa.component.html',
  styleUrls: ['./anasayfa.component.css']
})
export class AnasayfaComponent implements OnInit {

  registerMode: boolean = false;
  constructor() { }

  ngOnInit() {
  }
  registerToogle() {
    this.registerMode = !this.registerMode;
  }
  uyeligiIptalEt(durum: boolean) {

    this.registerMode = durum;
  }

}
