import { HttpErrorResponse } from "@angular/common/http";
import { ErrorHandler, Injectable, NgZone } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ErrorDialogComponent } from "./error-dialog/error-dialog.component";

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(
    private _snackBar: MatSnackBar,
    private _zone: NgZone,
    private _dialog: MatDialog
  ) {}

  handleError(error: any) {
    // Check if it's an error from an HTTP response
    if(!error) return;

    if (!(error instanceof HttpErrorResponse)) {
      error = error.rejection;
    }
    this._zone.run(() => {
      // this._snackBar.open(error?.message || 'Undefined client error');
      this._dialog.open(ErrorDialogComponent, {
        data: {
          message: error?.message ?? 'Undefined client error',
          error: error?.error ?? ''
        },
        disableClose: true
      });
    });

    console.error('Error from global error handler', error);
  }
}
