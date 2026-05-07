import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { Home } from './Home';

describe('Home', () => {
  it('renders Home Page heading', () => {
    render(<Home />);
    expect(screen.getByText(/Home Page/i)).toBeInTheDocument();
  });

  it('renders an h1 element', () => {
    render(<Home />);
    expect(screen.getByRole('heading', { level: 1 })).toBeInTheDocument();
  });
});

