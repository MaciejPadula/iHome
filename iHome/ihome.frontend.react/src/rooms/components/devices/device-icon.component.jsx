import React from 'react';

const DeviceIcon = ({deviceType,...props}) => {
    let src="";
    switch(deviceType){
        case 1:
            src = "../resources/images/rgbLamp.png";
            break;
        case 2:
            src = "../resources/images/temperature.png";
            break; 
    }
    return (
        <img className="device-image" draggable="false" src={src} />
    );
}

export default DeviceIcon;