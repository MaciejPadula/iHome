import React from 'react';

//components
import RGBLampControls from './rgb-lamp/rgb-lamp.component';
import TemperatureControls from './termometer/temperature.component';

const DeviceControls = ({deviceId, deviceType, deviceData, ...props}) => {
    if(deviceType==1){
        return (
            <RGBLampControls deviceId={deviceId} deviceData={deviceData} />
        );
    }
    else if(deviceType==2){
        return (
            <TemperatureControls deviceId={deviceId} deviceData={deviceData} />
        );
    }
    return (
        <div>Not recognised device type</div>
    );
}

export default DeviceControls;