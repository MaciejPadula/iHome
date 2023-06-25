#ifndef ESP8266DEVICEFACTORY_H
#define ESP8266DEVICEFACTORY_H

#include <IHomeDevice.h>
#include <IHomeDeviceHandler.h>
#include "ESP8266NetworkManager.h"
#include "ESP8266FirebaseClient.h"

class ESP8266DeviceFactory
{
public:
  static IHomeDevice create(ESP8266FirebaseConfiguration config, IHomeDeviceHandler *handler);
};

#endif