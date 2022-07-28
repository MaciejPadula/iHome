import React from 'react';
import { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

//api
import { getRooms, setDeviceRoom } from '../api/apiRequests';

//components
import RoomComponent from '../components/room.component';
import AddRoomModal from '../components/modals/add-room.component';
import Spinner from 'react-bootstrap/Spinner';
import { DragDropContext } from 'react-beautiful-dnd';

const RoomsPage = ({ ...props}) => {
  const [spinnerClassName, setSpinnerClassName] = useState("spinner-visible");

  const [roomsRequestData, setRoomsRequestData] = useState(JSON.stringify([]));
  const [rooms, setRooms] = useState([]);

  useEffect(() => {
    setInterval(() => {
      getRooms().then(res => setRoomsRequestData(JSON.stringify(res.data)));
    }, 1000);
  }, []);
  
  useEffect(() => {
    setRooms(JSON.parse(roomsRequestData));
  }, [roomsRequestData]);
  
  useEffect(() => {
    if(roomsRequestData!='[]'){
      setSpinnerClassName('invisible');
    }
  }, [rooms]);
  
  const onDragEnd = (result) => {
    setDeviceRoom(
      result.draggableId,
      result.destination.droppableId
    );
  };
  return (
    <>
      <AddRoomModal />
      <DragDropContext onDragEnd={onDragEnd}>
        <div id="rooms-container">
          <div className={spinnerClassName}>
            <Spinner animation="border" variant="primary">
              <span className="visually-hidden">Loading...</span>
            </Spinner>
          </div>
          {
            rooms.map((room) => <RoomComponent key={room.roomId} room={room} />)
          }
        </div>
      </DragDropContext>
    </>
  );
}

export default RoomsPage;