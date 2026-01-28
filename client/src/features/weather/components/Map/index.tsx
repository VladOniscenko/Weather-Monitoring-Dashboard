import React, { useState } from "react";
import { type WeatherStationDto } from "@/client";

import MapView from "./MapView.tsx";
import StatioDetails from "./StationDetails.tsx";

const Map: React.FC = () => {
  const [open, setOpen] = useState<boolean>(false);
  const [stationId, setStationId] = useState<string>();

  const handleStationSelection = (station: WeatherStationDto) => {
    setStationId(station.id);
    setOpen(true);
  };

  return (
    <>
      <MapView setStation={handleStationSelection} />
      {stationId && (
        <StatioDetails
          isOpen={open}
          stationId={stationId}
          close={() => setOpen(false)}
        />
      )}
    </>
  );
};

export default Map;
