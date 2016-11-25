import { Component, OnInit, Input } from '@angular/core';
import {Events} from './events';
import{Activity} from './activity';
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
  user : Users = new Users();
  token : Token;
  count: number = 0;
  test: string[];

  constructor(private ZenithService: ZenithService) { }

  getEvents(): void {
    this.ZenithService.getEvents().then(events => this.events = events);
  }
  getNextWeek(): void{
    this.ZenithService.getNewWeek(this.token.access_token, this.count).then(events => this.test = events)
  }
  ngOnInit(): void {
    this.getEvents();
  }

  verify(): void {
    //checks if anything was passed in
    //if (!user) { alert("BOO!") ;return; }
    //this.user.username
    //this.user.password

    // if (this.user.username == "ZenithAdmin"){
    //   this.test = true;
    // }

    this.ZenithService.getAPIToken( this.user.username, this.user.password).then(token => this.token = token)
    //.subscribe((result) => {
    //   if (result) {
    //      alert("yay")
    //   }
    // })
  }
}
