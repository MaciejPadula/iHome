#ifndef ESP8266NETWORKMANAGER_H
#define ESP8266NETWORKMANAGER_H

#include <ESP8266WiFi.h>
#include <NetworkManager.h>

class ESP8266NetworkManager : public NetworkManager
{
  String _macAddress;
  void waitForConnectionEstablished();
public:
  void setup() override;
  void connectToNetwork(String ssid, String password) override;
  void disconnectFromAllNetworks() override;
  void startAccessPoint() override;
  String getMacAddress() override;
};

#endif