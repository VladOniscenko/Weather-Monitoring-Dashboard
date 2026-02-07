import React, { useState, useMemo, useEffect, useCallback } from 'react';
import { MapContainer, TileLayer, Marker, useMap } from 'react-leaflet';
import L from 'leaflet';
import { Loader2 } from 'lucide-react';
import MarkerClusterGroup from 'react-leaflet-cluster';
import { type WeatherStationDto } from '@/client';

// Assets
import 'leaflet/dist/leaflet.css';
import styles from './MapView.module.css';

// Hooks & Context
import { useGeoLocation } from '@/hooks/useGeoLocation';
import { useAppTheme } from '@/context/ThemeContext';
import { useWeatherStationsCoordinates } from '@/features/weather/hooks/useWeatherStations';

const WORLD_BOUNDS: L.LatLngBoundsExpression = [
    [-90, -180],
    [90, 180],
];

const DEFAULT_CENTER: L.LatLngExpression = [51.9244, 4.4777]; // Rotterdam

// Memoized Marker Component to prevent re-attaching listeners
const StationMarker = React.memo(({ 
    station, 
    icon, 
    onSelect 
}: { 
    station: WeatherStationDto, 
    icon: L.DivIcon, 
    onSelect: (s: WeatherStationDto) => void 
}) => {
    // Memoize handlers to maintain referential equality
    const handlers = useMemo(() => ({
        click: () => onSelect(station),
    }), [station, onSelect]);

    return (
        <Marker
            position={[station.latitude!, station.longitude!]}
            icon={icon}
            eventHandlers={handlers}
        />
    );
});
StationMarker.displayName = 'StationMarker';

// Component to handle map movements
const MapController = ({ 
    onBoundsChange 
}: { 
    onBoundsChange: (zoom: number, bounds: L.LatLngBounds) => void 
}) => {
    const map = useMap();

    useEffect(() => {
        if (!map) return;

        const handleMapChange = () => {
            onBoundsChange(map.getZoom(), map.getBounds());
        };

        // Attach listeners manually for finer control if needed, 
        // or use useMapEvents. Debouncing logic happens in the parent.
        map.on('moveend', handleMapChange);
        map.on('zoomend', handleMapChange);

        // Initial bounds set
        handleMapChange();

        return () => {
            map.off('moveend', handleMapChange);
            map.off('zoomend', handleMapChange);
        };
    }, [map, onBoundsChange]);

    return null;
};

// Component to handle flying to user location once found
const LocationFlyTo = ({ position }: { position: { lat: number; lng: number } | null }) => {
    const map = useMap();
    const [hasFlown, setHasFlown] = useState(false);

    useEffect(() => {
        if (position && !hasFlown) {
            map.flyTo([position.lat, position.lng], 13, { duration: 2 });
            setHasFlown(true);
        }
    }, [position, map, hasFlown]);

    return null;
};

// Icon Generators
const createCustomIcon = (currentLocation: boolean = false) => {
    return L.divIcon({
        html: `<div class="${currentLocation ? styles.currentLocationMapMarker : styles.mapMarker}"></div>`,
        className: styles.customMarkerCluster,
        iconSize: [30, 30],
        iconAnchor: [15, 15],
    });
};

const createClusterCustomIcon = (cluster: any) => {
    return L.divIcon({
        html: `<div>${cluster.getChildCount()}</div>`,
        className: styles.customMarkerCluster,
        iconSize: [40, 40],
    });
};

// Main Component
interface MapViewProps {
    setStation: (station: WeatherStationDto) => void;
}

const MapView: React.FC<MapViewProps> = ({ setStation }: MapViewProps) => {
    const { position } = useGeoLocation();
    const { theme } = useAppTheme();
    
    // Memoize icons once
    const markerIcon = useMemo(() => createCustomIcon(false), []);
    const currentLocationMarkerIcon = useMemo(() => createCustomIcon(true), []);

    const [mapState, setMapState] = useState({
        zoom: 13,
        bounds: null as L.LatLngBounds | null,
    });

    // Debounce the map update to prevent API spamming
    const handleBoundsChange = useCallback((zoom: number, bounds: L.LatLngBounds) => {
        const timer = setTimeout(() => {
            setMapState({ zoom, bounds });
        }, 300);
        return () => clearTimeout(timer);
    }, []);

    const { data: stations, loading } = useWeatherStationsCoordinates({
        minLng: mapState.bounds?.getSouthWest().lng,
        maxLng: mapState.bounds?.getNorthEast().lng,
        minLat: mapState.bounds?.getSouthWest().lat,
        maxLat: mapState.bounds?.getNorthEast().lat,
        pageSize: 1000
    });

    const tileUrl = useMemo(() => {
        const variant = ['light', 'latte'].includes(theme) ? 'light' : 'dark';
        return `https://{s}.basemaps.cartocdn.com/${variant}_all/{z}/{x}/{y}{r}.png`;
    }, [theme]);

    return (
        <div className="relative h-screen w-full">
            {/* Loading Indicator Overlay */}
            {loading && (
                <div className="absolute bottom-8 left-1/2 -translate-x-1/2 z-[1000] flex items-center gap-2 px-4 py-2 bg-slate-900/90 text-white rounded-full shadow-lg backdrop-blur-md transition-all">
                    <Loader2 className="w-4 h-4 animate-spin text-blue-400" />
                    <span className="text-xs font-medium">Updating area...</span>
                </div>
            )}

            <MapContainer
                center={DEFAULT_CENTER}
                zoom={13}
                minZoom={3}
                maxZoom={18}
                maxBounds={WORLD_BOUNDS}
                className="h-full w-full"
                preferCanvas={true}
                attributionControl={false}
                zoomControl={false}
            >
                <TileLayer url={tileUrl} attribution="&copy; CARTO" />

                {/* Logic Components (No UI) */}
                <MapController onBoundsChange={handleBoundsChange} />
                <LocationFlyTo position={position} />

                {/* User Location */}
                {position && (
                    <Marker
                        key="user-loc"
                        position={[position.lat!, position.lng!]}
                        icon={currentLocationMarkerIcon}
                        zIndexOffset={1000}
                    />
                )}

                {/* Station Clusters */}
                <MarkerClusterGroup
                    chunkedLoading
                    maxClusterRadius={60}
                    spiderfyOnMaxZoom
                    iconCreateFunction={createClusterCustomIcon}
                    removeOutsideVisibleBounds={true}
                    animate={true}
                >
                    {stations?.items?.map((station) => (
                        <StationMarker 
                            key={station.id}
                            station={station}
                            icon={markerIcon}
                            onSelect={setStation}
                        />
                    ))}
                </MarkerClusterGroup>
            </MapContainer>
        </div>
    );
};

export default MapView;