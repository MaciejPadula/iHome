import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { WidgetsService } from 'src/app/services/data/widgets.service';
import { WidgetType } from 'src/app/shared/models/widget-type';

@Component({
  selector: 'app-add-widget-dialog',
  templateUrl: './add-widget-dialog.component.html',
  styleUrls: ['./add-widget-dialog.component.scss']
})
export class AddWidgetDialogComponent {
  public selectedType = WidgetType.Small;
  public showBorder = true;
  public readonly widgetTypes = [WidgetType.Small, WidgetType.Medium, WidgetType.Wide];

  constructor(
    public dialogRef: MatDialogRef<AddWidgetDialogComponent>,
    private _widgetsService: WidgetsService
  ) {}

  public confirm(): void {
    this.dialogRef.close({
      widgetType: this.selectedType,
      showBorder: this.showBorder
    });
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  public getWidgetTypeString(widgetType: WidgetType): string {
    return WidgetType[widgetType];
  }

  public get widgetStyle(): string {
    const additionalStyle = this.showBorder ? ' widget-border' : '';
    return this._widgetsService.resolveWidgetStyleByType(this.selectedType) + additionalStyle;
  }
}
