import React from 'react'
import {useState, useEffect} from 'react'
import axios from 'axios'
const TemperatureControls = ({deviceId, deviceData, ...props}) => {
    const data = JSON.parse(deviceData);
    const [temperature, setTemperature] = useState(data.temp);
    const [pressure, setPressure] = useState(data.pressure);
    
    setInterval(()=>{
        axios({
            method: 'post',
            url: '/api/rooms/GetDeviceData/'+deviceId,
        }).then(res => {
            setTemperature(res.data.temp);
            setPressure(res.data.pressure);
        });
    }, 200)

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