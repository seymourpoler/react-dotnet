import type { Forecast } from './Forecast';

export async function populateWeather(): Promise<Forecast[]> {
    const response = await fetch('weatherforecast');
    if (response.ok) {
        const data = await response.json();
        return data as Forecast[];
    }
    return [];
}

