import React from 'react';
import './styles/PrimaryButton.css';


const PrimaryButton = ({ onClick, children }) => {
    return (
        <button className="primary-button" onClick={onClick}>
            {children}
        </button>
    );
};

export default PrimaryButton;