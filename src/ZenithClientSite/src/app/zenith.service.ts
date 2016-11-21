import { Injectable } from '@angular/core';
import {Events} from './events';
import { Headers, Http, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class ZenithService {

  private BASE_URL = "http://localhost:5000/api/eventsAPI";
  private headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: Http) { }

  getEvents(): Promise<Events[]> {
    return this.http.get(this.BASE_URL)
      .toPromise()
      .then(response => response.json() as Events[])
      .catch(this.handleError);
  }
  //ERROR
  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}
