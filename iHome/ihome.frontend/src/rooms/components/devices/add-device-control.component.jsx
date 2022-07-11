import React from 'react'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import axios from 'axios'
const AddDeviceControl = ({device:{id, deviceId, deviceType}, roomId,...props}) => {
    const [deviceName, setDeviceName] = React.useState();
    const [validated, setValidated] = React.useState(false);
    const handleDeviceNameChange = (ev) => setDeviceName(ev.currentTarget.value);

    const addDevice = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        if(ev.currentTarget.checkValidity() === true){
            const data = {
                deviceId: deviceId,
                deviceName: deviceName,
                deviceType: deviceType,
                deviceData: '',
                roomId: roomId
            }
            switch(data.deviceType){
                case 1:
                    data.deviceData = '{"Red":255, "Green":255, "Blue":255, "State":1}';
                    break;
                case 2:
                    data.deviceData = '{"temp":0, "pressure": 0}';
                    break;
            }
            axios({
                method: 'post',
                url: '/api/rooms/adddevice/'+id,
                data: data
            }).then(res => {
                window.location.reload(false);
            });
        }
        setValidated(true);
        
    };

    return (
        <Form noValidate validated={validated} onSubmit={addDevice}>
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