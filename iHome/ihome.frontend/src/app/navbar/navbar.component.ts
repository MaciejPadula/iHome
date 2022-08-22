import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

//components
import { UserButtonComponent } from '../user-button/user-button.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(public auth: AuthService) { }

  ngOnInit(): void {
  }

}
