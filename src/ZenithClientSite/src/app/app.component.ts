import { Component, OnInit } from '@angular/core';
import {Events} from './events';
import {ZenithService} from './zenith.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ZenithService]
})
export class AppComponent implements OnInit {
  title = 'app works!';

  events: Events[];
  constructor(private ZenithService: ZenithService) { }

  getEvents(): void {
    this.ZenithService.getEvents().then(events => this.events = events);;
  }
  ngOnInit(): void {
    this.getEvents();
  }

}
