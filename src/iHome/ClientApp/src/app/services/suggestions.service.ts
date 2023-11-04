import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TimeModel } from '../shared/models/time-model';

@Injectable({
  providedIn: 'root'
})
export class SuggestionsService {
  private readonly _baseApiUrl = `${environment.authAudience}/Suggestion/`;

  constructor(
    private _api: HttpClient
  ) { }

  public getSuggestedHour(scheduleName: string): Observable<TimeModel> {
    return this._api.post<TimeModel>(`${this._baseApiUrl}GetSuggestedHour`,{
      scheduleName
    });
  }

  public getSuggestedDevices(scheduleName: string, scheduleTime: string): Observable<string[]> {
    return this._api.post<string[]>(`${this._baseApiUrl}GetSuggestedDevices`,{
      scheduleName,
      scheduleTime
    });
  }
}
