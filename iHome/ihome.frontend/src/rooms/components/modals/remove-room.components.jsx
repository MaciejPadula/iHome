import React from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import RemoveIcon from '../icons/remove.component'
import axios from 'axios'
const RemoveRoomModal = ({roomId, roomName, ...props}) => {
    const [show, setShow] = React.useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const removeRoom = () => {
        axios({
            method: 'post',
            url: '/api/rooms/removeroom/'+roomId,
        }).then(res => {
            handleClose();
        });
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
                    <Button variant="primary" onClick={removeRoom}>
                        Remove Room
                    </Button>
                </Modal.Footer>
            </Modal>
        </>

    );
}

export default RemoveRoomModal;