#include <Device.h>
#include <ArduinoHelpers.h>

class TestHandler : DeviceDataHandler {
private:
	int _pin;
	long long int lastMillis = 0;

public:
	TestHandler(int pin) {
		_pin = pin;
	}

	void setup() override {
		pinMode(2, OUTPUT);
	}

	String handle(unsigned long time, String inputData) override {
		if (time - lastMillis >= 1000) {
			ArduinoHelpers::togglePin(2);
			lastMillis = time;
		}

		return "";
	}
};

Device* device;

void setup() {
	auto handler = new TestHandler(2);
	device = new Device((DeviceDataHandler*)handler);
	device->setup(true);
}

void loop() {
	device->act(millis());
}
