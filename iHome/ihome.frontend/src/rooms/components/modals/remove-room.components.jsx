import React from 'react';
import { useState } from 'react';
import { removeRoom } from '../../api/apiRequests';

//components
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import RemoveIcon from '../icons/remove.component';

const RemoveRoomModal = ({roomId, roomName, ...props}) => {
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const removeRoomEvent = (ev) => {
        removeRoom(roomId).then(res => handleClose());
    };
    return (
        <>
            <Button variant="primary" className="rounded-0" onClick={handleShow}>
                <RemoveIcon />
            </Button>
            
            <Modal backdrop="static" show={show} onHide={handleClose} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Room Removal</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Do you want to remove this room?: {roomName}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={removeRoomEvent}>
                        Remove Room
                    </Button>
                </Modal.Footer>
            </Modal>
        </>

    );
}

export default RemoveRoomModal;