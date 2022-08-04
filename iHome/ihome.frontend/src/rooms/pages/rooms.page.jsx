import React from 'react';
import { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import $ from 'jquery';

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

  const readData = () => getRooms().then(res => setRoomsRequestData(JSON.stringify(res.data)));

  useEffect(() => {
    readData();
    setInterval(() => {
      readData();
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
    const drag = document.getElementById(result.draggableId);
    //document.getElementById(result.draggableId).style.display='none';
    if(result.destination.droppableId!=drag.dataset.roomid) {
      // const parent = document.getElementById(drag.dataset.roomid).children[0].children[2];
      // const dest = document.getElementById(result.destination.droppableId).children[0].children[2];

      drag.style.display = 'none';
      setDeviceRoom(
        result.draggableId,
        result.destination.droppableId
      );
      
      readData();
    }
    
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