import { Component, Inject } from '@angular/core';
import { InputDialogData } from './input-dialog-data';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-input-dialog',
  templateUrl: './input-dialog.component.html',
  styleUrls: ['./input-dialog.component.scss']
})
export class InputDialogComponent {
  public inputControl = new FormControl<string>('');

  constructor(
    public dialogRef: MatDialogRef<InputDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: InputDialogData,
  ) {}

  public get inputText() {
    return this.dialogData.inputText;
  }

  public get title() {
    return this.dialogData.title;
  }

  public saveChanges() {
    this.dialogRef.close(this.inputControl.value);
  }

  public onNoClick() {
    this.dialogRef.close(null);
  }
}
