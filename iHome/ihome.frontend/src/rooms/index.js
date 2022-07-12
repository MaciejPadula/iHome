import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { Router } from './router';


ReactDOM.render(
    <BrowserRouter basename="/Account/Rooms">
        <Router />
    </BrowserRouter>,
    document.getElementById('root'),
);
