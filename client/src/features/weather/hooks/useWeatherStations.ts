import { useEffect, useState } from 'react';
import {
    WeatherStationsService,
    type WeatherStationDtoPagedResponse,
    type StationCordinateDtoPagedResponse,
} from '@/client';

interface UseWeatherStationsParams {
    cityId?: string,
    name?: string,
    minLng?: number,
    maxLng?: number,
    minLat?: number,
    maxLat?: number,
    zoom?: number,
    page?: number,
    pageSize?: number,
    userId?: string,
    refreshKey?: number,
}

export function useWeatherStations(params?: UseWeatherStationsParams) {
    const [data, setData] = useState<WeatherStationDtoPagedResponse | null>(null);
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
                    params?.userId,
                    params?.page,
                    params?.pageSize,
                );

                if (!cancelled) {
                    setData(response.data ?? null);
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
    }, [params]);

    return { data, loading, error };
}

export function useWeatherStationsCoordinates(
    params?: UseWeatherStationsParams
) {
    const [data, setData] = useState<StationCordinateDtoPagedResponse | null>(null);
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
                        params?.userId,
                        params?.page,
                        params?.pageSize,
                    );

                if (!cancelled) {
                    setData(response.data || null);
                }
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchCoordinates();
        return () => {
            cancelled = true;
        };
    }, [params]);

    return { data, loading };
}
