import React from 'react';

//components
import DeviceIcon from './devices/device-icon.component';
import DeviceControls from './devices/device-controls.component';
import RenameDeviceModal from './modals/rename-device.component';
import { Draggable } from 'react-beautiful-dnd';

const DeviceComponent = ({device: {deviceId, deviceName, deviceType, deviceData, roomId}, isMaster, index, ...props}) => {
    if(isMaster){
        return(
            <Draggable key={deviceId} draggableId={deviceId} deviceId={deviceId} index={index}>
            {(provided, snapshot) => (
                <div className="card device-card" ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps}>
                    <RenameDeviceModal deviceId={deviceId} deviceName={deviceName}/>
                    <div className="card-body">
                    <DeviceIcon deviceType={deviceType} />
                    <div className="card-title">{deviceName}</div>
                        <DeviceControls deviceId={deviceId} deviceType={deviceType} deviceData={deviceData} />
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
                <DeviceControls deviceId={deviceId} deviceType={deviceType} deviceData={deviceData} />
            </div>
        </div>   
    );
};

export default DeviceComponent;