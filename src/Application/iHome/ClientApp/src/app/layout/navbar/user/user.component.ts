import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserComponent {
  constructor(
    private _auth: AuthService
  ) { }

  public login(): void {
    this._auth.loginWithRedirect();
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
