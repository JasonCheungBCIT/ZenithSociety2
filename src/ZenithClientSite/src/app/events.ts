import {Activity} from './activity';

export class Events {
  eventId:number;
  fromDate:Date;
  toDate:Date;
  isActive:boolean;
  createdBy:string;
  creationDate: string;
  activityId:number;
  activity: Activity; //null?
}
