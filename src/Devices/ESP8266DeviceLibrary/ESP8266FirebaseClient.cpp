#include "ESP8266FirebaseClient.h"

ESP8266FirebaseClient::ESP8266FirebaseClient(ESP8266FirebaseConfiguration configuration)
{
  _config.api_key = configuration.API_KEY;

  _auth.user.email = configuration.USER_EMAIL;
  _auth.user.password = configuration.USER_PASSWORD;

  _config.database_url = configuration.DATABASE_URL;

  Firebase.begin(&_config, &_auth);
  Firebase.reconnectWiFi(true);
}

String ESP8266FirebaseClient::getData(String macAddress)
{
  if (!Firebase.ready()) return "";

  Firebase.getString(_fbdo, "/" + macAddress);

  return _fbdo.to<const char *>();
}

void ESP8266FirebaseClient::setData(String macAddress, String data)
{
  if (!Firebase.ready()) {
    return;
  }

  Firebase.setString(_fbdo, "/" + macAddress, data);
}