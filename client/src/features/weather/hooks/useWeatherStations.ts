import { useEffect, useState } from 'react';
import {
    WeatherStationsService,
    type WeatherStationDto,
    type StationCordinateDto,
} from '@/client';

interface UseWeatherStationsParams {
    cityId?: string;
    name?: string;
    minLng?: number;
    maxLng?: number;
    minLat?: number;
    maxLat?: number;
    zoom?: number;
    page?: number;
    pageSize?: number;
}

export function useWeatherStations(params?: UseWeatherStationsParams) {
    const [data, setData] = useState<WeatherStationDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<unknown>(null);

    useEffect(() => {
        let cancelled = false;

        async function fetchStations() {
            setLoading(true);
            setError(null);

            try {
                const response = await WeatherStationsService.getAllStations(
                    params?.cityId,
                    params?.name,
                    params?.minLng,
                    params?.maxLng,
                    params?.minLat,
                    params?.maxLat,
                    params?.zoom,
                    params?.page,
                    params?.pageSize
                );

                if (!cancelled) {
                    setData(response.data ?? []);
                }
            } catch (err) {
                if (!cancelled) setError(err);
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchStations();

        return () => {
            cancelled = true;
        };
    }, [JSON.stringify(params)]);

    return { data, loading, error };
}

export function useWeatherStationsCoordinates(
    params?: UseWeatherStationsParams
) {
    const [data, setData] = useState<StationCordinateDto[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        let cancelled = false;

        async function fetchCoordinates() {
            setLoading(true);
            try {
                const response =
                    await WeatherStationsService.getAllStationsCordinates(
                        params?.cityId,
                        params?.name,
                        params?.minLng,
                        params?.maxLng,
                        params?.minLat,
                        params?.maxLat,
                        params?.zoom,
                        params?.page,
                        params?.pageSize
                    );

                if (!cancelled) {
                    setData(response.data || []);
                }
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchCoordinates();
        return () => {
            cancelled = true;
        };
    }, [JSON.stringify(params)]);

    return { data, loading };
}
