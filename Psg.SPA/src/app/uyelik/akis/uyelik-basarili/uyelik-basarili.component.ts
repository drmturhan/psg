import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-uyelik-basarili',
  templateUrl: './uyelik-basarili.component.html',
  styleUrls: ['./uyelik-basarili.component.css']
})
export class UyelikBasariliComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  aktivasyonEkraniniAc() {
    this.router.navigateByUrl('/epostaonayla');
  }
}
