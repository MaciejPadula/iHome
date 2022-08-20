import React from 'react';
import { useEffect } from 'react';
import { useState } from 'react';

//api
import { getDeviceData } from '../../../api/apiRequests';

const TemperatureControls = ({deviceId, deviceData, ...props}) => {
    const data = JSON.parse(deviceData);
    const [temperature, setTemperature] = useState(data.temp);
    const [pressure, setPressure] = useState(data.pressure);
    
    useEffect(() => {
        setInterval(()=>{
            getDeviceData(deviceId).then(res => {
                setTemperature(res.data.temp);
                setPressure(res.data.pressure);
            });
        }, 500);
    }, []);
    

    return (
        <div>
            <div>
                Temperature: {temperature}℃ 
            </div> 
            <div>
                Pressure: {pressure}hPa
            </div>
        </div>
    );
}

export default TemperatureControls;