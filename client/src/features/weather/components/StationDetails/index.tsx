import { useState } from 'react';
import { useStationDetails } from '@/features/weather/hooks/useStationDetails';
import styles from './StationDetails.module.css';

import StationHeader from './StationHeader';
import CurrentReading from './CurrentReading';
import ReadingHistory from './ReadingHistory';
import ErrorCard from './ErrorCard';
import LoadingCard from './LoadingCard';

interface StationInfoProps {
    isOpen: boolean;
    stationId: string;
    close: () => void;
}

export default function StationDetails({
    isOpen,
    stationId,
    close,
}: StationInfoProps) {
    const { station, readings, isLoading, error } =
        useStationDetails(stationId);

    const [selectedReadingId, setSelectedReadingId] = useState<string>();

    if (!isOpen) return null;
    if (isLoading) return <LoadingCard />;
    if (error || !station) return <ErrorCard error={error} />;

    const latestReading = readings?.[readings.length - 1];
    const activeReading =
        readings?.find((r) => r.id === selectedReadingId) ?? latestReading;

    if (!activeReading) return null;

    const isViewingHistory = activeReading.id !== latestReading?.id;

    return (
        <div className={`${styles.container} ${styles.floating} card`}>
            <StationHeader
                station={station}
                reading={activeReading}
                isViewingHistory={isViewingHistory}
                close={close}
            />

            <CurrentReading reading={activeReading} />

            <ReadingHistory
                readings={readings ?? []}
                selectedId={selectedReadingId}
                onSelect={setSelectedReadingId}
                isViewingHistory={isViewingHistory}
            />
        </div>
    );
}
