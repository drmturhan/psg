import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../_services/auth.service';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-yeniden-aktiflestir',
  templateUrl: './yeniden-aktiflestir.component.html',
  styleUrls: ['./yeniden-aktiflestir.component.css']
})
export class YenidenAktiflestirComponent implements OnInit {



  onayPostasiGonderildi = false;
  hataMesaji: string;
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  gonder(form: NgForm) {
    const kullaniciAdi = form.controls['kullaniciAdi'].value;
    const epostaAdresi = form.controls['epostaAdresi'].value;
    this.authService.yenidenAktivasyonkoduGonder(kullaniciAdi, epostaAdresi)
      .subscribe(sonuc => this.hataMesaji = null);
  }
}
