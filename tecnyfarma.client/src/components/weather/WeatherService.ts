import type { Forecast } from './Forecast';

export async function populateWeather(setForecasts: (data: Forecast[]) => void) {
    const response = await fetch('weatherforecast');
    if (response.ok) {
        const data = await response.json();
        setForecasts(data);
    }
}

