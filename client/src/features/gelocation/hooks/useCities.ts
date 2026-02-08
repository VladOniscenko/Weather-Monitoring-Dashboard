import { useEffect, useState } from 'react';
import { CitiesService, type CityDtoPagedResponse } from '@/client';

interface UseCitiesParams {
    name?: string;
    latitude?: string;
    longitude?: string;
    lookInsideBounds?: boolean;
    countryId?: string;
    page?: number;
    pageSize?: number;
}

export function useCities(params?: UseCitiesParams) {
    const [data, setData] = useState<CityDtoPagedResponse | null>(
        null
    );
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<unknown>(null);

    useEffect(() => {
        let cancelled = false;

        async function fetchStations() {
            setLoading(true);
            setError(null);

            try {
                const response = await CitiesService.getApiCities(
                    params?.name,
                    params?.latitude,
                    params?.longitude,
                    params?.lookInsideBounds,
                    params?.countryId,
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
    }, [JSON.stringify(params)]);

    return { data, loading, error };
}
