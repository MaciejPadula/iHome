#ifndef ESP8266FIREBASECLIENT_H
#define ESP8266FIREBASECLIENT_H

#include <FirebaseClient.h>
#include <ESP8266WiFi.h>
#include <FirebaseESP8266.h>

struct ESP8266FirebaseConfiguration
{
  String API_KEY;
  String DATABASE_URL;
  String USER_EMAIL;
  String USER_PASSWORD;
};

class ESP8266FirebaseClient : public FirebaseClient
{
  FirebaseData _fbdo;
  FirebaseAuth _auth;
  FirebaseConfig _config;
public:
  ESP8266FirebaseClient(ESP8266FirebaseConfiguration configuration);
  String getData(String macAddress) override;
  void setData(String macAddress, String data) override;
};

#endif