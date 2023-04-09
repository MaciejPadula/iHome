#include <Arduino.h>

class ArduinoHelpers {
public:
	static void togglePin(int pin) {
		digitalWrite(pin, digitalRead(pin));
	}
};