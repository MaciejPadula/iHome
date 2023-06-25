#ifndef IHOMEDEVICEHANDLER_H
#define IHOMEDEVICEHANDLER_H

#include <Arduino.h>

class IHomeDeviceHandler
{
public:
    virtual void setup() = 0;
    virtual String loop(long long millis, String incomingData) = 0;
};

#endif