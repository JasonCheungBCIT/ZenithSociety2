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


  constructor(private ZenithService: ZenithService) { }

  getEvents(): void {
    this.ZenithService.getEvents().then(events => this.events = events);

  }
  getNextWeek(num: number): void {
      this.count += num;
    this.ZenithService.getNewWeek(this.token.access_token, this.count).then(events => this.events = events)

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
