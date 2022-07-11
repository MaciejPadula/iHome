import React from 'react'
import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import EditIcon from '../icons/edit.component'
import Form from 'react-bootstrap/Form'
import axios from 'axios'
const RenameDeviceModal = ({deviceId, deviceName}) => {
    const [validated, setValidated] = React.useState(false);
    const [show, setShow] = React.useState(false);
    const [newDeviceName, setNewDeviceName] = React.useState(deviceName);
    const newDeviceNameHandle = (ev) => setNewDeviceName(ev.currentTarget.value);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const updateDeviceName = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            const data =
            {
                "deviceId": deviceId,
                "deviceName": newDeviceName
            };
            axios({
                method: 'post',
                url: '/api/rooms/renamedevice',
                data: data
            }).then(res => {
                handleClose();
            });
        }
        setValidated(true);
    };
    return (
        <>
            <Button variant="primary" className="btn-sm rounded-0" onClick={handleShow} style={{position:'absolute', left: '0px'}}>
                <EditIcon />
            </Button>
            
            <Modal show={show} onHide={handleClose} centered>
                <Form noValidate validated={validated} onSubmit={updateDeviceName}>
                    <Modal.Header closeButton>
                    <Modal.Title>Rename existing device</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form.Label htmlFor="deviceNameToRename">Device Name</Form.Label>
                        <Form.Control required minLength="3" onChange={newDeviceNameHandle} defaultValue={deviceName} type="text" id="deviceNameToRename" aria-describedby="deviceNameToRename" />
                        <Form.Control.Feedback type="invalid">
                            Device name should have at least 3 characters!
                        </Form.Control.Feedback>
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

export default RenameDeviceModal;