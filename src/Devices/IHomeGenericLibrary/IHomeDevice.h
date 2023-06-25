#ifndef IHOMEDEVICE_H
#define IHOMEDEVICE_H

#include "IHomeDeviceHandler.h"
#include "FirebaseClient.h"
#include "NetworkManager.h"

class IHomeDevice
{
    IHomeDeviceHandler *_handler;
    FirebaseClient *_firebaseClient;
    NetworkManager *_networkManager;
public:
    IHomeDevice(IHomeDeviceHandler *handler, FirebaseClient *firebaseClient, NetworkManager *networkManager);
    ~IHomeDevice();
    void setup();
    void loop(long long millis);
};

#endif