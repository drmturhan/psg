import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../_services/auth.service';

@Component({
  selector: 'app-eposta-onaylandi',
  templateUrl: './eposta-onaylandi.component.html',
  styleUrls: ['./eposta-onaylandi.component.css']
})
export class EpostaOnaylandiComponent implements OnInit {

  constructor(private aroute: ActivatedRoute, private router: Router, private authService: AuthService) { }
  sonuc = false;
  ngOnInit() {
    const kod = this.aroute.snapshot.queryParams['kod'];
    this.authService.kullaniciGuvenlikKoduDogrumu(kod).subscribe(sonuc => this.sonuc = sonuc = true, hata => this.sonuc = false);
  }

}
