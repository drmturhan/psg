import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http/';
import 'rxjs/add/operator/map';


@Injectable()
export class AuthService {
  baseUrl = 'http://localhost:55126/api/auth/';
  userToken: any;
  constructor(private http: Http) { }
  login(model: any) {

    return this.http.post(this.baseUrl + 'girisyap', model, this.requetsOptions()).map((response: Response) => {
      const user = response.json();
      if (user) {
        localStorage.setItem('token', user.tokenString);
        this.userToken = user.tokenString;
      }
    });
  }
  register(model: any) {
    return this.http.post(this.baseUrl + 'uyeol', model, this.requetsOptions());
  }
  private requetsOptions() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    const options = new RequestOptions({ headers: headers });
    return options;
  }
}
