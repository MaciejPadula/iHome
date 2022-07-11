import React from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import NewIcon from '../icons/new.component'
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import axios from 'axios';
const AddRoomModal = ({onAdded,...props}) => {
    const [validated, setValidated] = React.useState(false);
    const [show, setShow] = React.useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const [roomName, setRoomName] = React.useState("");
    const roomNameHandle = (ev) => setRoomName(ev.currentTarget.value);

    const [roomDescription, setRoomDescription] = React.useState("");
    const roomDescriptionHandle = (ev) => setRoomDescription(ev.currentTarget.value);

    const AddRoom = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            const data =
            {
                "roomName": roomName,
                "roomDescription": roomDescription,
                "roomImage": ""
            };
            axios({
                method: 'post',
                url: '/api/rooms/addroom',
                data: data
            }).then(res => {
                handleClose();
            });
        }
        setValidated(true);
        
    };
    return (
        <>
            <Button variant="primary" className="rounded-0" onClick={handleShow}>
                <NewIcon />
            </Button>
            <Modal show={show} onHide={handleClose} centered>
                <Form noValidate validated={validated} onSubmit={AddRoom}>
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
                        Save Changes
                    </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        </>

    );
}

export default AddRoomModal;