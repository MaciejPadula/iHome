import React from 'react';
import { useState, useEffect, useRef } from 'react';

//api
import { setDeviceData } from '../../../api/apiRequests';

//components
import { HexColorPicker } from 'react-colorful';
import Popover from 'react-bootstrap/Popover';
import Overlay from 'react-bootstrap/Overlay';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import BootstrapSwitchButton from 'bootstrap-switch-button-react';
import Form from 'react-bootstrap/Form';
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs';
import Button from 'react-bootstrap/Button';

import {RGBLampModes} from './rgb-lamp-modes';
import PopoverPicker from './PopoverPicker';



const RGBLampControls = ({deviceId, deviceData, ...props}) => {
    const data = JSON.parse(deviceData);
    const [color, setColor] = useState(rgbToHex(data.Red, data.Green, data.Blue));
    const [state, setState] = useState(Boolean(data.State));
    const [mode, setMode] = useState(data.Mode);

    const updateColor = (color) => {
        console.log(color);
        setColor(color);
    };
    const updateState = (ev) => {
        if(ev){
            setState(1);
        }
        else{
            setState(0);
        }
    };
    const [controls, setControls] = useState();

    const onSelect = (ev) => setMode(ev);

    useEffect(() => {
        if(state==1){
            setControls(<Tabs
                defaultActiveKey={mode}
                id="uncontrolled-tab-example"
                onSelect={onSelect}
            >
                <Tab eventKey={RGBLampModes.StaticColor} title="1">
                    <div>Static Color Mode</div>
                    <PopoverPicker color={color} onChange={updateColor}/>
                </Tab>
                <Tab eventKey={RGBLampModes.Rainbow} title="2">
                    <div>Rainbow Mode</div>
                </Tab>
                <Tab eventKey={RGBLampModes.Breathing} title="3">
                    <div>Breathing Color</div>
                    <PopoverPicker color={color} onChange={updateColor}/>
                </Tab>
            </Tabs>);
        }
        else{
            setControls();
        }
        const rgbColor = hexToRgb(color);
        setDeviceData(deviceId, {
            "Red": rgbColor.r,
            "Green": rgbColor.g,
            "Blue": rgbColor.b,
            "State": state,
            "Mode": mode
        });
    }, [color, state, mode])
    
    return (
        <div style={{width:"100%"}}>
            { controls }
            <Form.Group className="mb-3" controlId="exampleForm.ControlInput1" style={{display:"flex", flexDirection:"column", alignItems: "center"}}>
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