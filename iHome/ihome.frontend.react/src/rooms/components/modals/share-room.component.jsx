import React, { useState, useEffect } from 'react';

//api
import { GetRoomUsers } from '../../api/apiRequests';

//components
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { Coin, Share } from 'react-bootstrap-icons';
import ShareRoomForm from './share/room-share-form.component';
import RoomSharePerson from "./share/room-share-person.component";

const ShareRoomModal = ({roomId, masterUuid, ...props}) => {
    const [users, setUsers] = useState([]);

    const readUsers = () => GetRoomUsers(roomId).then(res => setUsers(res.data));

    useEffect(() => {
        readUsers();
    }, []);

    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    return (
        <>
            <Button variant="primary" className="rounded-0" onClick={handleShow}>
                <Share size={20} />
            </Button>
            <Modal show={show} onHide={handleClose} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Room sharing</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ShareRoomForm roomId={roomId} onSubmit={readUsers}/>
                    {users.map(user => <RoomSharePerson key={user.uuid} user={user} roomId={roomId} onRemove={readUsers}/>)}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                </Modal.Footer>
                
            </Modal>
        </>

    );
}

export default ShareRoomModal;