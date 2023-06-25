#ifndef IHOMEFIREBASECLIENT_H
#define IHOMEFIREBASECLIENT_H

#include <Arduino.h>

class FirebaseClient
{
public:
    virtual String getData(String macAddress) = 0;
    virtual void setData(String macAddress, String data) = 0;
};

#endif