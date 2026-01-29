import { useState, useEffect } from 'react';
import { WeatherStationsService, type WeatherStationDto } from '@/client';

export function useWeatherStation(id: string) {
    const [data, setData] = useState<WeatherStationDto>();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!id) return;

        let cancelled = false;

        async function fetchStation() {
            setLoading(true);

            try {
                const response =
                    await WeatherStationsService.getApiWeatherStations(id);
                if (!cancelled) setData(response.data);
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchStation();

        return () => {
            cancelled = true;
        };
    }, [id]);

    return { data, loading };
}
