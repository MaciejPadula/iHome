import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {
  private refreshSubject$ = new Subject<void>();

  public refresh$ = this.refreshSubject$.asObservable();

  public refresh(){
    this.refreshSubject$.next();
  }
}
