import React from 'react';
import {useState, useEffect} from 'react';
import { setDeviceData } from '../../api/apiRequests';

const RGBLampControls = ({deviceId, deviceData, ...props}) => {
    const data = JSON.parse(deviceData);
    const [color, setColor] = useState(rgbToHex(data.Red, data.Green, data.Blue));
    const [state, setState] = useState(data.State);
    const updateColor = (ev) => {
        setColor(ev.currentTarget.value);
    };
    const updateState = (ev) => {
        if(ev.currentTarget.checked){
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
    }, [color, state])

    return (
        <form id={deviceId} data-deviceid={deviceId}>
            <input onChange={updateColor} type="color" name="color" data-deviceid={deviceId} defaultValue={rgbToHex(data.Red, data.Green, data.Blue)}/>
            <div className="form-check form-switch">
                <input defaultChecked={state} onChange={updateState} className="form-check-input" type="checkbox" name="state" id="id1" data-deviceid={deviceId} role="switch" />
                <label className="label">Device State:</label>
            </div>
        </form>
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