import React from 'react';

//components
import DeviceIcon from './devices/device-icon.component';
import DeviceControls from './devices/device-controls.component';
import RenameDeviceModal from './modals/rename-device.component';
import { Draggable } from 'react-beautiful-dnd';
import { useState } from 'react';

const DeviceComponent = ({device: {deviceId, deviceName, deviceType, deviceData}, roomId, isMaster, index, ...props}) => {
    const [data, setData] = useState(deviceData);

    if(isMaster){
        return(
            <Draggable key={deviceId} draggableId={deviceId} deviceId={deviceId} index={index}>
            {(provided, snapshot) => (
                <div data-roomid={roomId} id={deviceId} className="card device-card" ref={provided.innerRef} {...provided.draggableProps}>
                    <RenameDeviceModal deviceId={deviceId} deviceName={deviceName}/>
                    <div className="card-body">
                        <DeviceIcon deviceType={deviceType} />
                        <div {...provided.dragHandleProps} className="card-title">{deviceName}</div>
                        <DeviceControls deviceId={deviceId} deviceType={deviceType} deviceData={data} />
                    </div>
                </div>   
            )}
            </Draggable>
        );
    }
    
    return(
        <div className="card device-card">
            <RenameDeviceModal deviceId={deviceId} deviceName={deviceName}/>
            <div className="card-body">
            <DeviceIcon deviceType={deviceType} />
            <div className="card-title">{deviceName}</div>
                <DeviceControls deviceId={deviceId} deviceType={deviceType} deviceData={data} />
            </div>
        </div>   
    );
};

export default DeviceComponent;