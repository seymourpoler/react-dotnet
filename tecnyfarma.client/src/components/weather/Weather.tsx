import { useEffect, useState } from 'react';
import type { Forecast } from './Forecast';
import { populateWeather } from "./WeatherService";

export function Weather(){
    const [forecasts, setForecasts] = useState<Forecast[]>();
    
    useEffect(() => {
        populateWeather(setForecasts);
    }, []);
    
    
    return (
        <div>
            <h1>Weather Page</h1>
            {forecasts === undefined
                ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
                : <table className="table table-striped" aria-labelledby="tableLabel">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        {forecasts.map(forecast =>
                            <tr key={forecast.date}>
                                <td>{forecast.date}</td>
                                <td>{forecast.temperatureC}</td>
                                <td>{forecast.temperatureF}</td>
                                <td>{forecast.summary}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            }
        </div>
    );
}