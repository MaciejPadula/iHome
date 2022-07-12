import React from 'react';
import {setDeviceRoom} from '../api/apiRequests';

//components
import AddDeviceModal from './modals/add-device.component';
import ShareRoomModal from './modals/share-room.component';
import RemoveRoomModal from './modals/remove-room.components';
import DeviceComponent from './device.component';

const RoomComponent = ({ room: {roomId, roomName, roomDescription, roomImage, devices}, ...props }) => {
    let rooms = [];
    const onDrop = (ev) => {
        ev.preventDefault();
            const deviceId = ev.dataTransfer.getData('deviceId');
            setDeviceRoom(deviceId, roomId).then(res => window.location.reload(false));
    };
    const onDragOver = (ev) => {
        ev.preventDefault();
    };
    
    return (
        <div onDrop={onDrop} onDragOver={onDragOver} className="card room-card">
            <div className="card-body">
                <div className="card-title">{roomName}</div>
                <p className="card-text">{roomDescription}</p>
                <div className="room-devices">
                    {
                        devices.map(device => <DeviceComponent key={device.deviceId} device={device} />)
                    }
                    <AddDeviceModal roomId={roomId}/>
                </div>
                <div className='edit-room-section'>
                    <ShareRoomModal roomId={roomId} />
                    <RemoveRoomModal roomId={roomId} roomName={roomName} />
                </div>
            </div>
        </div>
    );
};

export default RoomComponent;