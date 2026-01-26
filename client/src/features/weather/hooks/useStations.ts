import { useQuery } from '@tanstack/react-query';
import { WeatherStationsService, type WeatherStationDto } from '@/client';

type UseStationsParams = {
  cityId?: string;
  name?: string;
  page?: number;
  pageSize?: number;
  minLng?: number;
  maxLng?: number;
  minLat?: number;
  maxLat?: number;
  zoom?: number;
};

export const useStations = ({
  cityId,
  name,
  minLng,
  maxLng,
  minLat,
  maxLat,
  zoom,
  page = 1,
  pageSize = 100,
}: UseStationsParams = {}) => {
  return useQuery<WeatherStationDto[]>({
    queryKey: ['stations', { cityId, name, page, pageSize, minLng, maxLng, minLat, maxLat, zoom, }],
    queryFn: async () => {
      const response = await WeatherStationsService.getAllStations(
        cityId,
        name,
        minLng,
        maxLng,
        minLat,
        maxLat,
        zoom,
        page,
        pageSize,
      );

      return response.data ?? [];
    },

    placeholderData: (previousData) => previousData,
  });
};