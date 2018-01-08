import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';



@Component({
  selector: 'app-uyelik',
  templateUrl: './uyelik.component.html',
  styleUrls: ['./uyelik.component.css']
})
export class UyelikComponent implements OnInit {

  @Output() iptal = new EventEmitter();
  model: any = { kullaniciadi: '', sifre: '' }
  constructor(private authService: AuthService, private uyarici: AlertifyService) { }

  ngOnInit() {
  }
  uyeol() {
    this.authService.register(this.model).subscribe(() =>
      this.uyarici.success('Üyelik başarılı.'),
      error => this.uyarici.error(error));
  }
  vazgec() {
    this.iptal.emit(false);
    this.uyarici.message('Üyelik isteği iptal edildi...');
  }

}
