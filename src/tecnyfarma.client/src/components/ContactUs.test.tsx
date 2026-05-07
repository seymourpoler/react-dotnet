import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ContactUs } from './ContactUs';

describe('ContactUs', () => {
  it('renders Contact Page heading', () => {
    render(<ContactUs />);
    expect(screen.getByText(/Contact Page/i)).toBeInTheDocument();
  });
});

