import { Component, Input } from "@angular/core";
import { ControlValueAccessor } from "@angular/forms";

@Component({template: ''})
export abstract class DeviceBaseControlComponent<DeviceDataType> implements ControlValueAccessor {
  @Input() public value: string | null | undefined;

  protected get data(): DeviceDataType {
    if (!this.value) {
      return this.defaultData;
    }

    return JSON.parse(this.value) ?? this.defaultData;
  }

  protected set data(data: DeviceDataType) {
    this.setVal(JSON.stringify(data));
  }

  protected abstract get defaultData(): DeviceDataType;

  private setVal(val: string) {
    this.value = val;
    this.onChange(this.value);
    this.onTouch(this.value);
  }

  public writeValue(val: string): void {
    this.setVal(val);
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  public setDisabledState?(isDisabled: boolean): void {}/* eslint-disable-line */
  public onChange: any = () => {}/* eslint-disable-line */
  public onTouch: any = () => {}/* eslint-disable-line */
}
