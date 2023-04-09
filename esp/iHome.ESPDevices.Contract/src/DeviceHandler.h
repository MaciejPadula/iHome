#include <Arduino.h>

class DeviceDataHandler
{
public:
    virtual void setup();
    virtual String handle(unsigned long time, String inputData);
};