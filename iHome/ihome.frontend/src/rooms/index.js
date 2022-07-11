import React from 'react';
import ReactDOM from 'react-dom';
import thunk from 'redux-thunk';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import ApplicationReducer from './store';
import { Router } from './router';

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
let store = createStore(ApplicationReducer, composeEnhancers(applyMiddleware(thunk)));

ReactDOM.render(
    <BrowserRouter basename="/Account/Rooms">
        <Provider store={store}>
            <Router />
        </Provider>
    </BrowserRouter>,
    document.getElementById('root'),
);
