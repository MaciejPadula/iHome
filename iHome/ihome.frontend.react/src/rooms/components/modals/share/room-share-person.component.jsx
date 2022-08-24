import React from "react";

//api
import { removeRoomShare } from "../../../api/apiRequests";

//components
import { PersonX } from 'react-bootstrap-icons';

const RoomSharePerson = ({ roomId, user:{uuid, email}, onRemove, ...props }) => {
    const RemoveShare = (ev) => {
        removeRoomShare(roomId, uuid).then(res => {
            if(res.data.status == 200){
                onRemove();
            }
        });
    };
    return (
        <div style={{display:'flex', flexDirection:'row', padding:'0.3rem'}}>
            <div style={{cursor:'pointer', padding:'0.3rem'}}><PersonX onClick={RemoveShare} /></div>
            <div style={{padding:'0.3rem'}}>{ email }</div>
        </div>
    );
};

export default RoomSharePerson;