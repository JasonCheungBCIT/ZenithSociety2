import { Component, OnInit, Input } from '@angular/core';
import {Events} from './events';
import {Activity} from './activity';
import {ZenithService} from './zenith.service';
import {Users} from './Users';
import {Token} from './token';

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
  token: Token;
  count: number = 0;
  showLogin = false;
  fail = false;

  eventsKeys: string[] = [];                          // array of keys in the eventsDictionary 
  eventsDictionary: { [key: string]: Events[] } = {}; // [ Day => Event ]
  
  constructor(private ZenithService: ZenithService) { }

  getEvents(): void {
    this.ZenithService.getEvents().then(events => this.reformatData(events));
  }
  getNextWeek(num: number): void {
    console.log("Get next week");
    this.count += num;
    this.ZenithService.getNewWeek(this.token.access_token, this.count)
      .then(events => this.reformatData(events))
  }

  reformatData(data: Events[]) {
    console.log(data);
    console.log(data[0]);
    for (let e of data) {
      console.log(e);
      let day = new Date(e.fromDate)
      let dayKey = day.toDateString();
      
      if (!this.eventsKeys.find(s => s == dayKey))
        this.eventsKeys.push(dayKey);
      // Create or push 
      (this.eventsDictionary[dayKey] = this.eventsDictionary[dayKey] ? this.eventsDictionary[dayKey] : []).push(e);
    }

    console.log(this.eventsKeys)
    console.log(this.eventsDictionary);
  }

  ngOnInit(): void {
    this.getEvents();
  }

  verify(): void {
    this.ZenithService.getAPIToken(this.user.username, this.user.password).then(token => this.token = token)
    if(!this.token){
      this.fail = true;
    }
  }
}
