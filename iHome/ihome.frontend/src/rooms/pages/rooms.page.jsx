import React from 'react'
import { useState, useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import axios from 'axios'

//actions
import {RoomsActions} from '../store/rooms.store'

//components
import RoomComponent from '../components/room.component'
import AddRoomModal from '../components/modals/add-room.component'

const RoomsPage = ({ ...props}) => {
  const [roomsCount, setRoomsCount] = useState();
  const [devicesCount, setDevicesCount] = useState();

  setInterval(() => {
    axios({
      method: 'get',
      url: '/api/rooms/getroomscount',
    }).then(res => setRoomsCount(res.data.roomsCount));
    axios({
      method: 'get',
      url: '/api/rooms/getdevicescount',
    }).then(res => setDevicesCount(res.data.devicesCount));
  }, 1000);
  
  const dispatch = useDispatch();
  const {rooms} = useSelector(s=>s.Rooms);

  useEffect(() => {
    dispatch(RoomsActions.GetRooms());
  }, [roomsCount, devicesCount]);

  
  return (
    <>
      <AddRoomModal />
      <div id="rooms-container">
        {
          rooms.map((room) => <RoomComponent key={room.roomId} room={room} />)
        }
      </div>
    </>
  );
}

export default RoomsPage;