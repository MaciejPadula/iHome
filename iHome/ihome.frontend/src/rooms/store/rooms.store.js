import Axios from 'axios';

const SET_ROOMS = 'Rooms.SET_ROOMS';

const InitialState = {
    rooms: [],
};

const reducer = (state = InitialState, action) => {
    const { type, data } = action;

    switch (type) {
        case SET_ROOMS: {
            return {
                ...state,
                rooms: data,
            };
        }
    }
    return state;
};

export default reducer;

export const RoomsActions = {
    GetRooms: () => {
        return async (dispatch, getState) => {
            const response = await Axios.get(`/api/rooms/getrooms`);
            dispatch({
                type: SET_ROOMS,
                data: response.data,
            });
        };
    },
};
