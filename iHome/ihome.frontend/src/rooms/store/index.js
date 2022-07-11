import { combineReducers } from "redux";
import Rooms from "./rooms.store";

const ApplicationReducer = combineReducers({
    Rooms,
});

export default ApplicationReducer;
