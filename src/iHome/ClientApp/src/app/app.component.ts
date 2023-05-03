import { Component } from '@angular/core';
import { ChildrenOutletContexts } from '@angular/router';
import { slideInAnimation } from './animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    slideInAnimation
  ]
})
export class AppComponent {
  title = 'iHome';

  constructor(
    private _contexts: ChildrenOutletContexts
  ) {}

  public getRouteAnimationData() {
    return this._contexts.getContext('primary')?.route?.snapshot?.data?.['animation'];
  }
}
