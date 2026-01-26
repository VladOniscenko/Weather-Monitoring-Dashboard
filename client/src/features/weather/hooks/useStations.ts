import { useQuery } from '@tanstack/react-query';
import { WeatherStationsService, type WeatherStationDto } from '@/client';

type UseStationsParams = {
  cityId?: string;
  name?: string;
  page?: number;
  pageSize?: number;
};

export const useStations = ({
  cityId,
  name,
  page = 1,
  pageSize = 10,
}: UseStationsParams = {}) => {
  return useQuery<WeatherStationDto[]>({
    queryKey: ['stations', { cityId, name, page, pageSize }],
    queryFn: async () => {
      const response = await WeatherStationsService.getAllStations(
        cityId,
        name,
        page,
        pageSize,
      );

      return response.data ?? [];
    },

    placeholderData: (previousData) => previousData,
  });
};