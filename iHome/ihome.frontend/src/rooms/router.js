import React from 'react';
import { Switch, Route } from 'react-router-dom';

//pages
import RoomsPage from './pages/rooms.page'

export const Router = (props) => {
    return (
        <Switch>
            <Route exact path={'/'} component={RoomsPage} />
        </Switch>
    );
};
