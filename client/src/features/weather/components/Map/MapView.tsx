import React, { useState, useMemo } from "react";
import { MapContainer, TileLayer, Marker, useMapEvents } from "react-leaflet";
import L from "leaflet";
import { Loader2 } from "lucide-react";
import MarkerClusterGroup from "react-leaflet-cluster";
import { type WeatherStationDto } from "@/client";

// Assets
import "leaflet/dist/leaflet.css";
import styles from "./MapView.module.css";

// Hooks & Context
import { useGeoLocation } from "@/hooks/useGeoLocation";
import { useAppTheme } from "@/context/ThemeContext";
import { useWeatherStationsCoordinates } from "@/features/weather/hooks/useWeatherStations";

const WORLD_BOUNDS: L.LatLngBoundsExpression = [
  [-90, -180],
  [90, 180],
];

/** * Sub-component to handle map interaction logic
 */
const MapController = ({ onChange }: { onChange: (m: L.Map) => void }) => {
  const map = useMapEvents({
    moveend: () => onChange(map),
    zoomend: () => onChange(map),
  });
  return null;
};

const createCustomIcon = (currentLocation: boolean = false) => {
  return L.divIcon({
    html: `<div class="${currentLocation ? styles.currentLocationMapMarker : styles.mapMarker}"></div>`,
    className: styles.customMarkerCluster,
  });
};

const createClusterCustomIcon = (cluster: any) => {
  return L.divIcon({
    html: `<div>${cluster.getChildCount()}</div>`,
    className: styles.customMarkerCluster,
  });
};

interface MapViewProps {
  setStation: (station: WeatherStationDto) => void;
}

const MapView: React.FC<MapViewProps> = ({ setStation }: MapViewProps) => {
  const { position, error } = useGeoLocation();
  const { theme } = useAppTheme();
  const markerIcon = useMemo(() => createCustomIcon(), []);
  const currentLocationMarkerIcon = useMemo(() => createCustomIcon(true), []);

  // Unified state to prevent double-renders during debounce
  const [mapState, setMapState] = useState({
    zoom: 13,
    bounds: null as L.LatLngBounds | null,
  });

  const { data: stations, loading } = useWeatherStationsCoordinates({
    minLng: mapState.bounds?.getSouthWest().lng,
    maxLng: mapState.bounds?.getNorthEast().lng,
    minLat: mapState.bounds?.getSouthWest().lat,
    maxLat: mapState.bounds?.getNorthEast().lat,
    pageSize: 500,
  });

  const tileUrl = useMemo(() => {
    const variant = ["light", "latte"].includes(theme) ? "light" : "dark";
    return `https://{s}.basemaps.cartocdn.com/${variant}_all/{z}/{x}/{y}{r}.png`;
  }, [theme]);

  if (!position && !error) {
    return (
      <div className="flex items-center justify-center h-screen">
        <Loader2 className="animate-spin mr-2" /> Locating...
      </div>
    );
  }

  return (
    <div className="relative h-screen w-full">
      {loading && (
        <div className="absolute bottom-4 left-4 z-[1000] flex items-center gap-2 px-4 py-2 bg-slate-900/90 text-white rounded-full shadow-lg backdrop-blur-md">
          <Loader2 className="w-4 h-4 animate-spin text-app-accent" />
          <span className="text-xs font-medium">Updating...</span>
        </div>
      )}

      <MapContainer
        center={position ? [position.lat, position.lng] : [51.9244, 4.4777]} // set Rotterdam as default
        zoom={mapState.zoom}
        minZoom={3}
        maxZoom={18}
        maxBounds={WORLD_BOUNDS}
        className="h-full w-full"
        preferCanvas
        attributionControl={false}
      >
        <TileLayer url={tileUrl} attribution="&copy; CARTO" />

        <MapController
          onChange={(m) =>
            setMapState({ zoom: m.getZoom(), bounds: m.getBounds() })
          }
        />

        {position && (
          <Marker
            key="cloc"
            position={[position.lat!, position.lng!]}
            icon={currentLocationMarkerIcon}
          />
        )}

        <MarkerClusterGroup
          chunkedLoading
          maxClusterRadius={60}
          spiderfyOnMaxZoom
          iconCreateFunction={createClusterCustomIcon}
          removeOutsideVisibleBounds={true}
        >
          {stations?.map((station) => (
            <Marker
              key={station.id}
              position={[station.latitude!, station.longitude!]}
              icon={markerIcon}
              eventHandlers={{
                click: () => setStation(station),
              }}
            />
          ))}
        </MarkerClusterGroup>
      </MapContainer>
    </div>
  );
};

export default MapView;
