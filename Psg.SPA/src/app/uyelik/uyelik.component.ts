import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-uyelik',
  templateUrl: './uyelik.component.html',
  styleUrls: ['./uyelik.component.css']
})
export class UyelikComponent implements OnInit {

  @Output() iptal = new EventEmitter();
  model: any = { kullaniciadi: '', sifre: '' }
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  uyeol() {
    this.authService.register(this.model).subscribe(()=>{console.log("Üyelik başarılı")},error=>console.log(error));
  }
  vazgec() {
    this.iptal.emit(false);
  }

}
