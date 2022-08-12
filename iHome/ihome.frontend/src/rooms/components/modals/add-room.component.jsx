import React from 'react';

//api
import { addRoom } from '../../api/apiRequests';

//components
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { PlusSquareFill } from 'react-bootstrap-icons';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';

const AddRoomModal = ({onAdded, ...props}) => {
    const [validated, setValidated] = React.useState(false);
    const [show, setShow] = React.useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const [roomName, setRoomName] = React.useState("");
    const roomNameHandle = (ev) => setRoomName(ev.currentTarget.value);

    const [roomDescription, setRoomDescription] = React.useState("");
    const roomDescriptionHandle = (ev) => setRoomDescription(ev.currentTarget.value);

    const addRoomEvent = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            addRoom(roomName, roomDescription).then(res => handleClose());
        }
        setValidated(true);
        
    };
    return (
        <>
            <Button variant="primary" className="rounded-0" onClick={handleShow}>
                <PlusSquareFill />
            </Button>
            <Modal show={show} onHide={handleClose} centered>
                <Form noValidate validated={validated} onSubmit={addRoomEvent}>
                    <Modal.Header closeButton>
                    <Modal.Title>New Room</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <FloatingLabel
                            controlId="floatingInput"
                            label="Room Name"
                            className="mb-3"
                            style={{color:"#000000"}}
                        >
                            <Form.Control minLength="3" required type="text" placeholder="Enter room name" onChange={roomNameHandle}/>
                            <Form.Control.Feedback type="invalid">
                                Room name should have at least 3 characters!
                            </Form.Control.Feedback>
                        </FloatingLabel>
                        <FloatingLabel
                            controlId="floatingInput"
                            label="Room Description"
                            className="mb-3"
                            style={{color:"#000000"}}
                        >
                            <Form.Control as="textarea" placeholder="Leave a comment here" onChange={roomDescriptionHandle}/>
                        </FloatingLabel>
                    </Modal.Body>
                    <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" type="submit">
                        Add room
                    </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        </>

    );
}

export default AddRoomModal;