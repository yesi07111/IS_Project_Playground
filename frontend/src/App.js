import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import ActivitiesPage from './pages/ActivitiesPage';
import ResourcesPage from './pages/ResourcesPage';
import './styles/App.css';

function App() {
    return (
        <Router>
            <div className="App">
                <h1>Gestión del Parque Infantil</h1>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/activities" element={<ActivitiesPage />} />
                    <Route path="/resources" element={<ResourcesPage />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;