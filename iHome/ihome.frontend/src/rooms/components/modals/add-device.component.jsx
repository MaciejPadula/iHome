import React from 'react';
import { useState } from 'react';

//api
import { getDevicesToConfigure, getIpAddr } from '../../api/apiRequests';

//components
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Spinner from 'react-bootstrap/esm/Spinner';
import AddDeviceControl from '../devices/add-device-control.component';
import { PlusCircleDotted } from 'react-bootstrap-icons';

const AddDeviceModal = ({roomId, ...props}) => {
    const [show, setShow] = useState(false);
    const [spinnerClassName, setSpinnerClassName] = useState("invisible");

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
   
    const [devices, setDevices] = React.useState(<div></div>);
    const getAllDevices = () => {
        setSpinnerClassName("spinner-visible");

        getIpAddr().then(res => {
            getDevicesToConfigure(res.data).then(res => {
                let outputContainer = 
                <div>
                    {
                        res.data.map(device => <AddDeviceControl key={device.deviceId} roomId={roomId} device={device} />)
                    }
                </div>;
                setSpinnerClassName("invisible");
                setDevices(outputContainer);
            });
        });

        
    };
    
    return (
        <>
            <div className="card device-card new-device-card" onClick={handleShow}>
                <div className="card-body">
                    <div className="new-device-button-icon">
                        <PlusCircleDotted size={30} />
                    </div>
                </div>
                
            </div>
            <Modal show={show} onHide={handleClose} centered>
                <Modal.Header closeButton>
                    <Modal.Title>Available devices</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <div className={spinnerClassName} style={{height:"auto"}}>
                    <Spinner animation="border" variant="primary">
                        <span className="visually-hidden">Loading...</span>
                    </Spinner>
                </div>
                    {devices}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={getAllDevices}>
                        Scan for available devices
                    </Button>
                </Modal.Footer>
            </Modal>
        </>

    );
}

export default AddDeviceModal;