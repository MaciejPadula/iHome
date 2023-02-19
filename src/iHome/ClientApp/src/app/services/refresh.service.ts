import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {
  private refreshSubject$ = new Subject<void>();
  private refreshDeviceSubject$ = new Subject<string>();

  public refresh$ = this.refreshSubject$.asObservable();
  public refreshDevice$ = this.refreshDeviceSubject$.asObservable();

  public refresh(){
    this.refreshSubject$.next();
  }

  public refreshDevice(deviceId: string){
    this.refreshDeviceSubject$.next(deviceId);
  }
}
