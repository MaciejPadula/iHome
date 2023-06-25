#include "ESP8266DeviceFactory.h"
#include <ArduinoJson.h>
#include <Adafruit_NeoPixel.h>
#include "ESP8266Credentials.h"

struct Color {
  int red = 0;
  int green = 0;
  int blue = 0;
};

Color deserializeColorJson(String json) {
  Color color;
  StaticJsonDocument<200> doc;
  deserializeJson(doc, json);

  if(!doc["state"]) {
    return color;
  }
  
  color.red = doc["red"];
  color.green = doc["green"];
  color.blue = doc["blue"];

  return color;
}

class RgbLampHandler : public IHomeDeviceHandler {
  int _ledCount;
  Adafruit_NeoPixel *_leds;

  void setColor(uint32_t color){
    for(int i=0; i < _ledCount; i++) {
      _leds->setPixelColor(i, color);
    }
    _leds->show();
  }

public:
  RgbLampHandler(int ledPin, int ledCount) {
    _ledCount = ledCount;
    _leds = new Adafruit_NeoPixel(ledCount, ledPin, NEO_GRB + NEO_KHZ800);
  }

  void setup() override {
    _leds->begin();
    setColor(0);
  }

  String loop(long long millis, String incomingData) override {
    if(incomingData != "") {
      auto color = deserializeColorJson(incomingData);
      setColor(Adafruit_NeoPixel::Color(color.red, color.green, color.blue));
    }

    return "";
  }
};

auto device = ESP8266DeviceFactory::create(getFirebaseConfiguration(), new RgbLampHandler(4, 2));

void setup() {
  device.setup();
}

void loop() {
  device.loop(millis());
}
