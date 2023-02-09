import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserComponent implements OnInit {

  constructor(private _auth: AuthService) { }

  ngOnInit(): void {
  }

  public login(): void {
    this._auth.loginWithPopup();
  }

  public logout(): void {
    this._auth.logout();
  }

  public get isAuthenticated(): Observable<boolean> {
    return this._auth.isAuthenticated$
  }

  public get user(): Observable<User | null | undefined> {
    return this._auth.user$;
  }
}
