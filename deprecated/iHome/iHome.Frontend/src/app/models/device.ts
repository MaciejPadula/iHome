export interface Device {
    id: string,
    type: number,
    name: string,
    data: string,
    roomId: string
}

const defaultDevice: Device = {
    id: "",
    type: 0,
    name: "",
    data: "",
    roomId: ""
};

export {
    defaultDevice
};