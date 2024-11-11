import React from 'react';
import { render, screen } from '@testing-library/react';
import PrimaryButton from '../components/PrimaryButton';

describe('PrimaryButton', () => {
    test('renders button with children', () => {
        render(<PrimaryButton>Click Me</PrimaryButton>);
        const buttonElement = screen.getByRole('button', { name: /click me/i });
        expect(buttonElement).toBeInTheDocument();
    });
});