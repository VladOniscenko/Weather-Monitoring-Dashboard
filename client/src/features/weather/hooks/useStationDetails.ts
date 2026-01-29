import { useState, useEffect } from 'react';
import {
    type WeatherReadingDto,
    type WeatherStationDto,
    WeatherReadingService,
    WeatherStationsService,
} from '@/client';

export const useStationDetails = (stationId: string) => {
    const [station, setStation] = useState<WeatherStationDto | null>(null);
    const [readings, setReadings] = useState<WeatherReadingDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!stationId) return;

        const fetchAllData = async () => {
            setIsLoading(true);
            try {
                const [stationResponse, readingsResponse] = await Promise.all([
                    WeatherStationsService.getApiWeatherStations(stationId),
                    WeatherReadingService.getReadings(
                        undefined, // Start
                        undefined, // End
                        stationId, // StationId
                        0, // Page
                        100 // PageSize
                    ),
                ]);

                setStation(stationResponse.data ?? null);
                setReadings(readingsResponse.data ?? []);
            } catch (err) {
                setError('Failed to load station details');
                console.error(err);
            } finally {
                setIsLoading(false);
            }
        };

        fetchAllData();
    }, [stationId]);

    return { station, readings, isLoading, error };
};
