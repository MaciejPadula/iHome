#include "DeviceHandler.h"
#include <Arduino.h>

class Device 
{
private:
    DeviceDataHandler *_handler;
public:
    Device(DeviceDataHandler *handler);
    ~Device();
    void setup(bool isSerialPortActive);
    void act(unsigned long time);
};
