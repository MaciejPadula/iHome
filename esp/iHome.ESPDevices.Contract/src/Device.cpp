#include "Device.h"

Device::Device(DeviceDataHandler *handler)
{
  _handler = handler;
}

Device::~Device()
{
  delete(_handler);
}

void Device::setup(bool isSerialPortActive)
{
  _handler->setup();

  if(!isSerialPortActive) return;

  Serial.begin(115200);
}

void Device::act(unsigned long time)
{
  Serial.println(time);
  _handler->handle(time, "");
}