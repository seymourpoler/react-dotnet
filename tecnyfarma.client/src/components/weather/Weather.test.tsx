import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { Weather } from './Weather';

const forecasts = [
	{
		date: '2026-05-06',
		temperatureC: 20,
		temperatureF: 68,
		summary: 'Sunny',
	},
	{
		date: '2026-05-07',
		temperatureC: 22,
		temperatureF: 71.6,
		summary: 'Cloudy',
	},
];

describe('Weather', () => {
	let fetch: ReturnType<typeof vi.spyOn>;

	beforeEach(() => {
		fetch = vi
			.spyOn(global, 'fetch')
			.mockResolvedValue({
				ok: true,
				json: async () => forecasts,
			} as any);
	});

	afterEach(() => {
		fetch.mockRestore();
		vi.resetAllMocks();
	});

	it('renders loading state initially', () => {
		render(<Weather />);
		expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
	});

	it('renders weather table after data loads', async () => {
		render(<Weather />);
		await waitFor(() => expect(screen.getByText('Sunny')).toBeInTheDocument());
		expect(screen.getByText('Cloudy')).toBeInTheDocument();
		expect(screen.getByText('2026-05-06')).toBeInTheDocument();
		expect(screen.getByText('20')).toBeInTheDocument();
		expect(screen.getByText('68')).toBeInTheDocument();
		expect(screen.getByText('2026-05-07')).toBeInTheDocument();
		expect(screen.getByText('22')).toBeInTheDocument();
		expect(screen.getByText('71.6')).toBeInTheDocument();
	});
});
