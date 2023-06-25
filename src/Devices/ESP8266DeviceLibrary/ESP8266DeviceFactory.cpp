#include "ESP8266DeviceFactory.h"

IHomeDevice ESP8266DeviceFactory::create(ESP8266FirebaseConfiguration config, IHomeDeviceHandler *handler)
{
  return IHomeDevice(handler, new ESP8266FirebaseClient(config), new ESP8266NetworkManager());
}