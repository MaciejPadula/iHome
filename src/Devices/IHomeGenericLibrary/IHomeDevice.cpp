#include "IHomeDevice.h"

IHomeDevice::IHomeDevice(IHomeDeviceHandler *handler, FirebaseClient *firebaseClient, NetworkManager *networkManager)
{
    _handler = handler;
    _firebaseClient = firebaseClient;
    _networkManager = networkManager;
}

IHomeDevice::~IHomeDevice()
{
    delete _handler;
    delete _firebaseClient;
    delete _networkManager;
}

void IHomeDevice::setup()
{
    _handler->setup();
    _networkManager->setup();
}

void IHomeDevice::loop(long long millis)
{
    auto macAddress = _networkManager->getMacAddress();
    auto receivedData = _firebaseClient->getData(macAddress);

    auto dataToSend = _handler->loop(millis, receivedData);
    if(dataToSend == 0 || dataToSend == "") return;

    _firebaseClient->setData(macAddress, dataToSend);
}