import React from 'react';

//components
import DeviceIcon from './devices/device-icon.component';
import DeviceControls from './devices/device-controls.component';
import RenameDeviceModal from './modals/rename-device.component';

const DeviceComponent = ({device: {deviceId, deviceName, deviceType, deviceData, roomId}, ...props}) => {
    const onDragStart = (ev) => {
        ev.dataTransfer.setData('deviceId', deviceId);
        setTimeout(() => ev.target.style.display = 'none', 0);
    };
    const onDragEnd = (ev) => {
        ev.target.style.display = "flex";
    };
    
    return(
        <div onDragStart={onDragStart} onDragEnd={onDragEnd} className="card device-card" draggable="true" data-deviceid="5C:CF:7F:B0:F7:31">
            <RenameDeviceModal deviceId={deviceId} deviceName={deviceName}/>
            <div className="card-body">
                <DeviceIcon deviceType={deviceType} />
                <div className="card-title">{deviceName}</div>
                <DeviceControls deviceId={deviceId} deviceType={deviceType} deviceData={deviceData} />
            </div>
        </div>
    );
};

export default DeviceComponent;