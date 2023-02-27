import { TestBed } from '@angular/core/testing';

import { DeviceDataHelper } from './device-data.helper';

describe('DeviceDataHelperService', () => {
  let service: DeviceDataHelper;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeviceDataHelper);
  });

  it('getColorFromRgbLampData when rgb data with float values provided should return correct color', () => {
    //Arrange
    const data = {
      red: 121.52312312,
      green: 32.0123012,
      blue: 200.00,
      state: true,
      mode: 0
    }

    //Act
    const color = service.getColorFromRgbLampData(data).getRgba();

    //Assert
    expect(color.red).toBe(122);
    expect(color.green).toBe(32);
    expect(color.blue).toBe(200);
  });

  it('getRgbLampDataWithAndOverrideState when state and data provided should change only state', () => {
    //Arrange
    const data = {
      red: 122,
      green: 32,
      blue: 200.00,
      state: true,
      mode: 0
    }

    const newState = false;

    //Act
    const newData = service.getRgbLampDataWithAndOverrideState(data, newState);

    //Assert
    expect(newData.red).toBe(data.red);
    expect(newData.green).toBe(data.green);
    expect(newData.blue).toBe(data.blue);
    expect(newData.mode).toBe(data.mode);

    expect(newData.state).toBe(newState);
  });
});
