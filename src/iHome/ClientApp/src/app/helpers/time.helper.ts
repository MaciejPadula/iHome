import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class TimeHelper {
  public timeFormatPipe(hour: number, minute: number){
    if(minute >= 60) {
      hour += minute / 60;
      minute %= 60;
    }

    if(hour >= 24) {
      hour %= 24;
    }

    return `${this.formatSegment(hour)}:${this.formatSegment(minute)}`;
  }

  public getLocalDateFromUTC(hour: number, minute: number){
    const localDate = new Date(Date.UTC(2023, 3, 27, hour, minute));

    return this.timeFormatPipe(localDate.getHours(), localDate.getMinutes());
  }

  public getDateFromTime(hour: number, minute: number, second = 0) {
    return new Date(2023, 3, 27, hour, minute, second);
  }

  public getDateFromTimeString(timeString: string){
    const date = timeString.split(':');

    return this.getDateFromTime(parseInt(date[0]), parseInt(date[1]));
  }

  public getUtcDateStringFromLocalTimeString(timeString: string) {
    const date = this.getDateFromTimeString(timeString);

    return this.timeFormatPipe(date.getUTCHours(), date.getUTCMinutes());
  }

  private formatSegment(segment: number){
    const segmentString = segment.toFixed(0);
    return segmentString.length < 2 ? `0${segmentString}` : segmentString;
  }
}
