import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogData } from './confirm-dialog-data';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _dialogData: ConfirmDialogData,
  ) {}

  public get additionalText() {
    return this._dialogData.additionalText ?? '';
  }

  public get title() {
    return this._dialogData.title;
  }

  public OnConfirmClicked() {
    this.dialogRef.close(true);
  }

  public onNoClick() {
    this.dialogRef.close(false);
  }
}
