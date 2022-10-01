import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'index-button',
  templateUrl: './index-button.component.html',
  styleUrls: ['./index-button.component.scss']
})
export class IndexButtonComponent {
  @Input() drawer: any;
}
