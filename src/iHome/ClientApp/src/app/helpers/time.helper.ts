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

    return `${this.formatSegment(hour)}:${this.formatSegment(minute)}`
  }

  private formatSegment(segment: number){
    const segmentString = segment.toFixed(0);
    return segmentString.length < 2 ? `0${segmentString}` : segmentString;
  }
}
