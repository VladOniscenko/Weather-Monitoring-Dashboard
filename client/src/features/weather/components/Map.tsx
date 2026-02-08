import { useState } from 'react';
import { type WeatherStationDto } from '@/client';

import MapView from './MapView';
import StationDetails from './StationDetails';

export default function Map() {
    const [stationId, setStationId] = useState<string | null>(null);

    const handleStationSelection = (station: WeatherStationDto) => {
        setStationId(station.id ?? null);
    };

    return (
        <>
            <MapView setStation={handleStationSelection} />
            <StationDetails
                stationId={stationId}
                close={() => setStationId(null)}
            />
        </>
    );
}
