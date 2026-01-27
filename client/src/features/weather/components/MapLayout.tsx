import React, { useState, useMemo } from 'react';
import { MapContainer, TileLayer, Marker, useMapEvents } from 'react-leaflet';
import L from 'leaflet';
import { Loader2 } from 'lucide-react';
import MarkerClusterGroup from 'react-leaflet-cluster';

// Assets
import markerIcon2x from 'leaflet/dist/images/marker-icon-2x.png';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';
import 'leaflet/dist/leaflet.css';
import styles from "./map-layout.module.css";

// Hooks & Context
import { useGeoLocation } from '@/hooks/useGeoLocation';
import { useAppTheme } from '@/context/ThemeContext';
import { useStationCordinates } from '@/features/weather/hooks/useStations';

L.Icon.Default.mergeOptions({
  iconRetinaUrl: markerIcon2x,
  iconUrl: markerIcon,
  shadowUrl: markerShadow,
});

const WORLD_BOUNDS: L.LatLngBoundsExpression = [[-90, -180], [90, 180]];

/** * Sub-component to handle map interaction logic 
 */
const MapController = ({ onChange }: { onChange: (m: L.Map) => void }) => {
  const map = useMapEvents({
    moveend: () => onChange(map),
    zoomend: () => onChange(map),
  });
  return null;
};

const createCustomIcon = () => {
  return L.divIcon({
    className: '',
    html: `<div class="${styles.mapMarker}" style="
        --pulse-delay: ${Math.random() * 2}s;
        --pulse-duration: ${1.8 + Math.random()}s;
      "></div>`,
  });
}

const MapLayout: React.FC = () => {
  const { position, error } = useGeoLocation();
  const { theme } = useAppTheme();

  // Unified state to prevent double-renders during debounce
  const [mapState, setMapState] = useState({
    zoom: 13,
    bounds: null as L.LatLngBounds | null
  });

  const { data: stations, isFetching } = useStationCordinates({
    minLng: mapState.bounds?.getSouthWest().lng,
    maxLng: mapState.bounds?.getNorthEast().lng,
    minLat: mapState.bounds?.getSouthWest().lat,
    maxLat: mapState.bounds?.getNorthEast().lat,
    pageSize: 2000
  });

  const tileUrl = useMemo(() => {
    const variant = ['light', 'latte'].includes(theme) ? 'light' : 'dark';
    return `https://{s}.basemaps.cartocdn.com/${variant}_all/{z}/{x}/{y}{r}.png`;
  }, [theme]);

  if (error) return <div className="flex items-center justify-center h-screen text-red-500">Error: {error}</div>;
  if (!position) return <div className="flex items-center justify-center h-screen"><Loader2 className="animate-spin mr-2" /> Locating...</div>;

  return (
    <div className="relative h-screen w-full">
      {(isFetching) && (
        <div className="absolute bottom-4 left-4 z-[1000] flex items-center gap-2 px-4 py-2 bg-slate-900/90 text-white rounded-full shadow-lg backdrop-blur-md">
          <Loader2 className="w-4 h-4 animate-spin text-app-accent" />
          <span className="text-xs font-medium">Updating...</span>
        </div>
      )}

      <MapContainer
        center={[position.lat, position.lng]}
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
          onChange={(m) => setMapState({ zoom: m.getZoom(), bounds: m.getBounds() })}
        />

        <MarkerClusterGroup 
          chunkedLoading 
          maxClusterRadius={60}
          spiderfyOnMaxZoom={true}
        >
          {stations?.map((station) => (
            <Marker
              key={station.id}
              position={[station.latitude!, station.longitude!]}
              icon={createCustomIcon()}
              eventHandlers={{ click: () => console.log(station) }}
            />
          ))}
        </MarkerClusterGroup>

      </MapContainer>
    </div>
  );
};

export default MapLayout;