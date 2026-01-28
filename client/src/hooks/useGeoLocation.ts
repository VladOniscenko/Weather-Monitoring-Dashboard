import { useState, useEffect } from 'react';

interface Position {
  lat: number;
  lng: number;
}

export const useGeoLocation = () => {
  const [position, setPosition] = useState<Position | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!navigator.geolocation) {
      setError('Geolocation is not supported by your browser.');
      return;
    }

    const success = (pos: GeolocationPosition) => {
      const { latitude, longitude } = pos.coords;
      setPosition({ lat: latitude, lng: longitude });
    };

    const fail = (err: GeolocationPositionError) => {
      switch (err.code) {
        case err.PERMISSION_DENIED:
          setError('User denied the request for Geolocation.');
          break;
        case err.POSITION_UNAVAILABLE:
          setError('Location information is unavailable.');
          break;
        case err.TIMEOUT:
          setError('The request to get user location timed out.');
          break;
        default:
          setError('An unknown error occurred.');
          break;
      }
    };

    navigator.geolocation.getCurrentPosition(success, fail);
  }, []);

  return { position, error };
};
