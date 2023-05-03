import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  constructor(private _auth: AuthService, private _router: Router, public auth: AuthService) {
    this._auth.isAuthenticated$.subscribe(userAuthenticated => {
      if(userAuthenticated){
        this._router.navigate(['/rooms']);
      }
    });
    
  }

  public ngOnInit(): void {
  }

}
