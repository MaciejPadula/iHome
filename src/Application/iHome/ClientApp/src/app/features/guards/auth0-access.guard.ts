import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Auth0AccessGuard {
  constructor(
    private _auth: AuthService
  ) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
      const isAuth = await firstValueFrom(this._auth.isAuthenticated$);
      if(!isAuth){
        this._auth.loginWithRedirect({
          appState: { target: state.url }
        });
      }
      return isAuth;
  }
}
