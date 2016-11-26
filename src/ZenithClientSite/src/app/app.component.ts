import { Component, OnInit, Input } from '@angular/core';
import {Events} from './events';
import {Activity} from './activity';
import {ZenithService} from './zenith.service';
import {Users} from './Users';
import {NewUser} from './new-user';
import {Token} from './token';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ZenithService]
})
export class AppComponent implements OnInit {
  title = 'app works!';

  events: Events[];
  user: Users = new Users();
  newUser: NewUser = new NewUser();
  token: Token;
  count: number = 0;
  showLogin = false;
  showRegister = false;
  fail = false;
  error: string = "";
  errorBool = false;
  what : string[] = [];
  eventsKeys: string[] = [];                          // array of keys in the eventsDictionary
  eventsDictionary: { [key: string]: Events[] } = {}; // [ Day => Event ]

  constructor(private ZenithService: ZenithService) { }

  getEvents(): void {
    this.ZenithService.getEvents().then(events => this.reformatData(events));
  }
  getNextWeek(num: number): void {
    console.log("Get next week");
    this.eventsKeys = [];
    this.eventsDictionary = {};
    this.count += num;
    this.ZenithService.getNewWeek(this.token.access_token, this.count)
      .then(events => this.reformatData(events))
  }



  ngOnInit(): void {
    this.getEvents();
  }

  verify(username:string, password: string): void {
    this.ZenithService
      .getAPIToken(username, password)
      .then(token => this.onVerifyResult(token))
      .catch(error => this.handleRegisterError(error))
  }

  register(): void{
    this.ZenithService
      .register(this.newUser)
      .then(response => this.onRegisterResult(response))
      .catch(error => this.handleRegisterError(error));
  }

  onRegisterResult(newUser: string[]){
    //console.log(newUser)
    console.log(this.newUser.FirstName);
    this.verify(this.newUser.Username, this.newUser.Password);
  }

  handleRegisterError(error: any) {
    console.log(error);
    this.errorBool = true;
    this.error = "Attempt Failed, Please Try Again!"
  }

  onVerifyResult(token: Token) {
        // console.log(this.fail);
        // if(this.token){
        //   this.fail = true;
        //   console.log(this.fail);
        // }
        this.token = token;
        this.ZenithService
          .getRolePermission(this.token.access_token)
          .then(result => this.onRoleResult(result))
          .catch(error => this.handleRegisterError(error))
  }

  onRoleResult(result: string[]){
    console.log(result);
  }

  reformatData(data: Events[]) {
    //console.log(data);
    //console.log(data[0]);
    for (let e of data) {
      //console.log(e);
      let fromDate = new Date(e.fromDate);
      let toDate = new Date(e.toDate);

      // reformat data
      let dayKey = fromDate.toDateString();

      if (!this.eventsKeys.find(s => s == dayKey))
        this.eventsKeys.push(dayKey);
      // Create or push
      (this.eventsDictionary[dayKey] = this.eventsDictionary[dayKey] ? this.eventsDictionary[dayKey] : []).push(e);
    }

    //console.log(this.eventsKeys)
    console.log(this.eventsDictionary);
  }


}
