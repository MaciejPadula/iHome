import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import { Router } from './router';
import { createRoot } from 'react-dom/client';

const container = document.getElementById('root');
const root = createRoot(container); // createRoot(container!) if you use TypeScript
root.render(
    <BrowserRouter basename="/Account/Rooms">
        <Router />
    </BrowserRouter>
);