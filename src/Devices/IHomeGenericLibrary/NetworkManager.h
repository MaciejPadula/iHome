#ifndef IHOMENETWORKMANAGER_H
#define IHOMENETWORKMANAGER_H

#include <Arduino.h>

class NetworkManager
{
public:
    virtual void setup();
    virtual void connectToNetwork(String ssid, String password);
    virtual void disconnectFromAllNetworks();
    virtual void startAccessPoint();
    virtual String getMacAddress();
};

#endif