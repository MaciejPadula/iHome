import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {
  private refreshSubject$ = new Subject<void>();

  public refresh$ = this.refreshSubject$.asObservable();

  constructor() { }

  public refresh(){
    this.refreshSubject$.next();
  }
}
