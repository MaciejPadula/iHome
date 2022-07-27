import React from 'react';
import { useState,useEffect } from 'react';

//api
import { setDeviceRoom } from '../api/apiRequests';

//components
import AddDeviceModal from './modals/add-device.component';
import ShareRoomModal from './modals/share-room.component';
import RemoveRoomModal from './modals/remove-room.components';
import DeviceComponent from './device.component';
import { ShareFill } from 'react-bootstrap-icons';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';


const RoomComponent = ({ room: {roomId, roomName, roomDescription, roomImage, devices, uuid, masterUuid}, ...props }) => {
    const [editSection, setEditSection] = useState(
        <div className='edit-room-section'>
            <ShareRoomModal roomId={roomId} />
            <RemoveRoomModal roomId={roomId} roomName={roomName} />
        </div>
    );
    const renderSharedRoomTooltip = (props) => (
        <Tooltip {...props}>
            Other user shared you this room
        </Tooltip>
    );
    useEffect(() => {
        if(masterUuid!=uuid){
            setEditSection(
            <div className='edit-room-section'>
                <OverlayTrigger
                    placement="bottom"
                    delay={{ show: 250, hide: 400 }}
                    overlay={renderSharedRoomTooltip}
                >
                    <ShareFill size={20} />
                </OverlayTrigger>
            </div>
            );
        }
    },[])
    
    const onDrop = (ev) => {
        ev.preventDefault();
        const deviceId = ev.dataTransfer.getData('deviceId');
        setDeviceRoom(deviceId, roomId);
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
                {editSection}
            </div>
        </div>
    );
};

export default RoomComponent;