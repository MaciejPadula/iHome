import React from 'react';

//api
import { addDevice } from '../../api/apiRequests';

//components
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const AddDeviceControl = ({device:{id, deviceId, deviceType}, roomId,...props}) => {
    const [deviceName, setDeviceName] = React.useState();
    const [validated, setValidated] = React.useState(false);
    const handleDeviceNameChange = (ev) => setDeviceName(ev.currentTarget.value);

    const addDeviceEvent = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            addDevice(id, deviceId, deviceName, deviceType, roomId)
            .then(res => window.location.reload(false));
        }
        setValidated(true);
        
    };

    return (
        <Form noValidate validated={validated} onSubmit={addDeviceEvent}>
            <div style={{display:"flex",flexDirection:"row"}}>
                <Form.Control minLength="3" required type="text" placeholder="Enter device name" onChange={handleDeviceNameChange}/>
                <Form.Control.Feedback type="invalid">
                    Room name should have at least 3 characters!
                </Form.Control.Feedback>

                <Button type="submit">Add</Button>
            </div>
        </Form>
    );
};

export default AddDeviceControl;