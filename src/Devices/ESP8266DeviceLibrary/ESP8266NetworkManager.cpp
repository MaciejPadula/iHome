#include "ESP8266NetworkManager.h"

void ESP8266NetworkManager::setup()
{
#ifdef WIFI_IS_OFF_AT_BOOT
  enableWiFiAtBootTime();
#endif

  WiFi.mode(WIFI_STA);
  WiFi.begin();
  waitForConnectionEstablished();
  this->_macAddress = WiFi.macAddress();
}

void ESP8266NetworkManager::connectToNetwork(String ssid, String password)
{
  WiFi.begin(ssid, password);
  waitForConnectionEstablished();
  WiFi.setAutoConnect(true);
}

void ESP8266NetworkManager::disconnectFromAllNetworks()
{
  ESP.eraseConfig();
  ESP.restart();
}

String ESP8266NetworkManager::getMacAddress()
{
  return _macAddress;
}

void ESP8266NetworkManager::startAccessPoint()
{
  // if(WiFi.isConnected()) {
  //     WiFi.disconnect();
  // }

  IPAddress localIp(192, 168, 1, 1);
  IPAddress gateway(192, 168, 1, 1);
  IPAddress subnet(255, 255, 255, 0);
  WiFi.softAPConfig(localIp, gateway, subnet);
  WiFi.softAP("iHomeConfig - " + _macAddress);
}

void ESP8266NetworkManager::waitForConnectionEstablished()
{
  while (WiFi.status() != WL_CONNECTED) { delay(1); }
}