import axios from 'axios';

function getRooms() {
    return axios.get('/api/Rooms/GetRooms');
}

function addRoom(roomName, roomDescription){
    return axios({
        method: 'post',
        url: '/api/rooms/addroom',
        data: {
            "roomName": roomName,
            "roomDescription": roomDescription,
            "roomImage": ""
        }
    });
}

function removeRoom(roomId){
    return axios({
        method: 'post',
        url: '/api/rooms/removeroom/'+roomId,
    });
}

function shareRoom(roomId, friendsEmail) {
    return axios({
        method: 'post',
        url: '/api/rooms/ShareRoom',
        data: {
            "roomId": roomId,
            "email": friendsEmail
        }
    });
}

function getRoomsCount() {
    return axios({
        method: 'get',
        url: '/api/rooms/getroomscount',
    });
}

function addDevice(id, deviceId, deviceName, deviceType, roomId){
    let deviceData = '';
    switch(deviceType){
        case 1:
            deviceData = '{"Red":255, "Green":255, "Blue":255, "State":1}';
            break;
        case 2:
            deviceData = '{"temp":0, "pressure": 0}';
            break;
    }
    return axios({
        method: 'post',
        url: '/api/rooms/adddevice/'+id,
        data: {
            deviceId: deviceId,
            deviceName: deviceName,
            deviceType: deviceType,
            deviceData: deviceData,
            roomId: roomId
        }
    });
}

function renameDevice(deviceId, deviceName){
    return axios({
        method: 'post',
        url: '/api/rooms/renamedevice',
        data: {
            "deviceId": deviceId,
            "deviceName": deviceName
        }
    });
}

function getDevicesCount() {
    return axios({
        method: 'get',
        url: '/api/rooms/getdevicescount',
    });
}

function getDeviceData(deviceId){
    return axios({
        method: 'post',
        url: '/api/rooms/GetDeviceData/'+deviceId,
    });
}

function setDeviceData(deviceId, deviceData){
    return axios({
        method: 'post',
        url: '/api/rooms/SetDeviceData',
        data: {
            "deviceId": deviceId,
            "deviceData": JSON.stringify(deviceData)
        }
    })
}

function setDeviceRoom(deviceId, roomId){
    return axios({
        method: 'post',
        url: '/api/rooms/setdeviceroom',
        data: {
            "deviceId": deviceId,
            "roomId": roomId
        },
    });
}

function getUserId(){
    return axios({
        method: 'get',
        url: '/api/Rooms/GetUserId',
    });
}

function getDevicesToConfigure(){
    return axios({
        method: 'get',
        url: '/api/Rooms/GetDevicesToConfigure',
    });
}

export {
    getRooms, 
    addRoom,
    removeRoom,
    shareRoom, 
    getRoomsCount, 
    //getDevices
    addDevice,
    renameDevice,
    getDevicesCount, 
    getDeviceData,
    setDeviceData,
    setDeviceRoom,
    getUserId,
    getDevicesToConfigure,
};