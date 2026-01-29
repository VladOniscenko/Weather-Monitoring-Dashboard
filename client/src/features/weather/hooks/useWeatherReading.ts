import { useState, useEffect } from 'react';
import { WeatherReadingService, type WeatherReadingDto } from '@/client';

export function useWeatherReadings(stationId?: string) {
    const [data, setData] = useState<WeatherReadingDto[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!stationId) return; // nothing to fetch

        let cancelled = false;

        async function fetchReadings() {
            setLoading(true);

            try {
                // call the correct API method
                const response = await WeatherReadingService.getReadings(
                    undefined, // start
                    undefined, // end
                    stationId, // stationId
                    0, // page
                    100 // pageSize, adjust as needed
                );

                if (!cancelled) {
                    setData(response.data ?? []); // extract the array from the response
                }
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchReadings();

        return () => {
            cancelled = true;
        };
    }, [stationId]);

    return { data, loading };
}
