import { useQuery } from '@tanstack/react-query';
import { WeatherStationsService } from '@/client';

export const useStations = () => {
  return useQuery({
    queryKey: ['stations'], 
    // The generator gives you a typed function ready to use
    queryFn: () => WeatherStationsService.getApiWeatherStations() 
  });
};