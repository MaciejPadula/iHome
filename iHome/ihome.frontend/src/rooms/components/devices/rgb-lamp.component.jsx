import React from 'react';
import { useState, useEffect } from 'react';

//api
import { setDeviceData } from '../../api/apiRequests';

//components
import BootstrapSwitchButton from 'bootstrap-switch-button-react';
import Form from 'react-bootstrap/Form';

const RGBLampControls = ({deviceId, deviceData, ...props}) => {
    const data = JSON.parse(deviceData);
    const [color, setColor] = useState(rgbToHex(data.Red, data.Green, data.Blue));
    const [state, setState] = useState(Boolean(data.State));
    const updateColor = (ev) => {
        setColor(ev.currentTarget.value);
    };
    const updateState = (ev) => {
        if(ev){
            setState(1);
        }
        else{
            setState(0);
        }
    };
    useEffect(() => {
        const rgbColor = hexToRgb(color);
        setDeviceData(deviceId, {
            "Red": rgbColor.r,
            "Green": rgbColor.g,
            "Blue": rgbColor.b,
            "State": state
        });
        console.log(state);
    }, [color, state])

    return (
        <div>
            <input onChange={updateColor} type="color" name="color" defaultValue={rgbToHex(data.Red, data.Green, data.Blue)}/>
            <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                <Form.Label>Lamp state: </Form.Label>
                <BootstrapSwitchButton 
                    checked={state} 
                    onstyle="primary" 
                    onChange={updateState} 
                    size="xs" 
                />
            </Form.Group>
        </div>
    );
}
function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}
  
function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}
function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}
export default RGBLampControls;