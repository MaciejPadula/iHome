#include "ESP8266FirebaseClient.h"

ESP8266FirebaseConfiguration getFirebaseConfiguration() {
  ESP8266FirebaseConfiguration config;
  config.API_KEY = "";
  config.DATABASE_URL = "";
  config.USER_EMAIL = "";
  config.USER_PASSWORD = "";
  return config;
}

