export interface TermometerData {
    temp: number;
    pressure: number;
}

const defaultTermometerData: TermometerData = {
    temp: 0,
    pressure: 0
};

export {defaultTermometerData}