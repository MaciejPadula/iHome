import React from 'react';
import { useState, useEffect } from 'react';

//api


//components
import AddDeviceModal from './modals/add-device.component';
import ShareRoomModal from './modals/share-room.component';
import RemoveRoomModal from './modals/remove-room.components';
import DeviceComponent from './device.component';
import { ShareFill } from 'react-bootstrap-icons';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import { Draggable, Droppable } from 'react-beautiful-dnd';

const RoomComponent = ({ room: {roomId, roomName, roomDescription, roomImage, devices, uuid, masterUuid}, ...props}) => {
    const [owner, setOwner] = useState(false);

    useEffect(() => {
        setOwner(masterUuid==uuid);
    },[]);
    return (
        <Droppable droppableId={`${roomId}`} direction="horizontal">
            {(provided, snapshot) => (
                <div id={roomId} className="card room-card" {...provided.droppableProps} ref={provided.innerRef}>
                    <div className="card-body">
                        <div className="card-title">{roomName}</div>
                        <p className="card-text">{roomDescription}</p>
                        <div className="room-devices">
                            {
                                devices.map((device,index) => <DeviceComponent key={device.deviceId} roomId={roomId} isMaster={masterUuid==uuid} device={device} index={index}/>)
                            }
                            {provided.placeholder}
                            <AddDeviceModal roomId={roomId}/>
                        </div>
                        { 
                            owner && 
                            (<div className='edit-room-section'>
                                <ShareRoomModal roomId={roomId} masterUuid={masterUuid}/>
                                <RemoveRoomModal roomId={roomId} roomName={roomName} />
                            </div>)
                        }
                        {
                            !owner && 
                            (<div className='edit-room-section'>
                                <OverlayTrigger
                                    placement="bottom"
                                    delay={{ show: 250, hide: 400 }}
                                    overlay={<Tooltip>ther user shared you this room</Tooltip>}
                                >
                                    <ShareFill size={20} />
                                </OverlayTrigger>
                            </div>)
                        }
                    </div>
                </div>
            )}
        </Droppable>
    );
};

export default RoomComponent;