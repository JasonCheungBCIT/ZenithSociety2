import { Injectable } from '@angular/core';
import {Events} from './events';
import {Token} from './token';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class ZenithService {

  private BASE_URL = "http://localhost:5000";
  private headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: Http) { }

  getEvents(): Promise<Events[]> {
    return this.http.get(this.BASE_URL + '/api/eventsAPI')
      .toPromise()
      .then(response => response.json() as Events[])
      .catch(this.handleError);
  }

  getAPIToken(username: string, password: string): Promise<Token> {
    var body = 'grant_type=password&username='+username+'&password='+password;
    var headers3 = new Headers();
    headers3.append('Content-Type', 'application/x-www-form-urlencoded');

    return this.http
      .post(this.BASE_URL + '/connect/token', body, { headers: headers3 })
      .toPromise()
      .then(response => response.json() as Token)
      .catch(this.handleError);
  }

  getNewWeek(token: string, num: number): Promise<string[]>{
    var body = '';
    var headers2 = new Headers({ 'Accept': 'application/json' });
    headers2.append('Authorization', 'Bearer '+ token);
    //headers2.append('Access-Control-Allow-Origin', 'http://localhost:4200')
    //headers2.append('Content-Type', 'application/json')
    let options = new RequestOptions({ headers: headers2 });

    return this.http.get(this.BASE_URL + '/api/eventsAPI/2',options)
      .toPromise()
      .then(response => response.json() as string[])
      .catch(this.handleError);
  }

  // getnext(): Promise<Token[]> {
  //   return this.http.get(this.BASE_URL + '/api/eventsAPI/2')
  //     .toPromise()
  //     .then(response => response.json() as Token[])
  //     .catch(this.handleError);
  // }


  //ERROR
  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}
