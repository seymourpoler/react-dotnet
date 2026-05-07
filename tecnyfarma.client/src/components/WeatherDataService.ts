import type { Forecast } from './weather/Forecast';

export async function populateWeatherData(setForecasts: (data: Forecast[]) => void) {
    const response = await fetch('weatherforecast');
    if (response.ok) {
        const data = await response.json();
        setForecasts(data);
    }
}


