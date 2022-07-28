import React from 'react';
import { useState,useEffect } from 'react';

//api


//components
import AddDeviceModal from './modals/add-device.component';
import ShareRoomModal from './modals/share-room.component';
import RemoveRoomModal from './modals/remove-room.components';
import DeviceComponent from './device.component';
import { ShareFill } from 'react-bootstrap-icons';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import { Droppable } from 'react-beautiful-dnd';

const RoomComponent = ({ room: {roomId, roomName, roomDescription, roomImage, devices, uuid, masterUuid}, ...props}) => {
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
    },[]);
    return (
        <Droppable droppableId={`${roomId}`}>
            {(provided, snapshot) => (
                <div className="card room-card" {...provided.droppableProps} ref={provided.innerRef}>
                    <div className="card-body">
                        <div className="card-title">{roomName}</div>
                        <p className="card-text">{roomDescription}</p>
                        <div className="room-devices">
                            {
                                devices.map((device,index) => <DeviceComponent isMaster={masterUuid==uuid} key={device.deviceId} device={device} index={index}/>)
                            }
                            {provided.placeholder}
                            <AddDeviceModal roomId={roomId}/>
                        </div>
                        {editSection}
                    </div>
                </div>
            )}
            
        </Droppable>
    );
};

export default RoomComponent;