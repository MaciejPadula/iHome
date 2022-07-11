import React from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import NewIcon from '../icons/new.component'
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import axios from 'axios';
const ShareRoomModal = ({roomId,...props}) => {
    const [validated, setValidated] = React.useState(false);
    const [show, setShow] = React.useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const [email, setEmail] = React.useState();
    const handleEmailChange = (ev) => {
        setEmail(ev.currentTarget.value);
    };

    const ShareRoom = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            const data =
            {
                "roomId": roomId,
                "email": email
            };
            axios({
                method: 'post',
                url: '/api/rooms/ShareRoom',
                data: data
            }).then(res => {
                handleClose();
                console.log(res);
            });
        }
        setValidated(true);
        
    };
    return (
        <>
            <Button variant="primary" className="rounded-0" onClick={handleShow}>
                Test
            </Button>
            <Modal show={show} onHide={handleClose} centered>
                <Form noValidate validated={validated} onSubmit={ShareRoom}>
                    <Modal.Header closeButton>
                    <Modal.Title>New Room</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <FloatingLabel
                            controlId="floatingInput"
                            label="Friend email"
                            className="mb-3"
                            style={{color:"#000000"}}
                        >
                            <Form.Control required type="email" placeholder="Enter friend email" onChange={handleEmailChange}/>
                            <Form.Control.Feedback type="invalid">
                                Invalid email!
                            </Form.Control.Feedback>
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

export default ShareRoomModal;