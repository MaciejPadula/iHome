import React from 'react';
import RemoveRoomModal from './modals/remove-room.components';
import DeviceComponent from './device.component';
import axios from 'axios';
import AddDeviceModal from './modals/add-device.component';
import ShareRoomModal from './modals/share-room.component';
const RoomComponent = ({ room: {roomId, roomName, roomDescription, roomImage, devices}, ...props }) => {
    let rooms = [];
    const onDrop = (ev) => {
        ev.preventDefault();
            const deviceId = ev.dataTransfer.getData('deviceId');
            axios({
                method: 'post',
                url: '/api/rooms/setdeviceroom',
                data: {
                    "deviceId": deviceId,
                    "roomId": roomId
                },
            }).then(res => window.location.reload(false));
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
                <RemoveRoomModal roomId={roomId} roomName={roomName} />
                <ShareRoomModal roomId={roomId} />
            </div>
        </div>
    );
};

export default RoomComponent;