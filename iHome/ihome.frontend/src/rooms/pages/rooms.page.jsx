import React from 'react';
import { useState, useEffect } from 'react';

//api
import {getRooms, getRoomsCount, getDevicesCount} from '../api/apiRequests';

//components
import RoomComponent from '../components/room.component';
import AddRoomModal from '../components/modals/add-room.component';
import Spinner from 'react-bootstrap/Spinner';

const RoomsPage = ({ ...props}) => {
  const [roomsCount, setRoomsCount] = useState(0);
  const [devicesCount, setDevicesCount] = useState(0);
  const [spinnerClassName, setSpinnerClassName] = useState("spinner-visible");
  const [rooms, setRooms] = useState(<div></div>);

  setInterval(() => {
    getRoomsCount().then(res => setRoomsCount(res.data.roomsCount));
    getDevicesCount().then(res => setDevicesCount(res.data.devicesCount));
  }, 1000);

  useEffect(() => {
    getRooms().then(res => {
        let outputContainer = 
        <div>
            {
                res.data.map(room => <RoomComponent key={room.roomId} room={room} />)
            }
        </div>;
        setSpinnerClassName("invisible");
        setRooms(outputContainer);
    });
    
  }, [roomsCount, devicesCount]);

  
  return (
    <>
      <AddRoomModal />
      <div id="rooms-container">
        <div className={spinnerClassName}>
          <Spinner animation="border" variant="primary">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </div>
        {rooms}
      </div>
    </>
  );
}

export default RoomsPage;