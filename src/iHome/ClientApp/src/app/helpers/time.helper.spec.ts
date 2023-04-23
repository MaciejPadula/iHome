import { TestBed } from "@angular/core/testing";
import { TimeHelper } from "./time.helper";

describe('TimeHelper', () => {
  let service: TimeHelper;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeHelper);
  });

  it('timeFormatPipe when hour and minute are one digit numbers should produce correct string', () => {
    //Arrange
    const hour = 1;
    const minute = 1;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("01:01");
  });

  it('timeFormatPipe when hour is one digit and minute is two digit should produce correct string', () => {
    //Arrange
    const hour = 1;
    const minute = 12;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("01:12");
  });

  it('timeFormatPipe when hour is two digit and minute is one digit should produce correct string', () => {
    //Arrange
    const hour = 12;
    const minute = 1;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("12:01");
  });

  it('timeFormatPipe when hour and minute are two digit numbers should produce correct string', () => {
    //Arrange
    const hour = 12;
    const minute = 12;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("12:12");
  });

  it('timeFormatPipe when provided minute value is greater or equals 60 should increase hour', () => {
    //Arrange
    const hour = 12;
    const minute = 62;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("13:02");
  });

  it('timeFormatPipe when provided minute value is greater or equals 60 and hour are greater than 23 should increase hours and get reminder', () => {
    //Arrange
    const hour = 23;
    const minute = 62;

    //Act
    const time = service.timeFormatPipe(hour, minute);

    //Assert
    expect(time).toBe("00:02");
  });
});
