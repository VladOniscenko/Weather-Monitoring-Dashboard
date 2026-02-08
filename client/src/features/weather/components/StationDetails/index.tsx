import { useState } from 'react';
import { useStationDetails } from '@/features/weather/hooks/useStationDetails';
import styles from './StationDetails.module.css';

import StationHeader from './StationHeader';
import CurrentReading from './CurrentReading';
import ReadingHistory from './ReadingHistory';
import ErrorCard from './ErrorCard';
import LoadingCard from './LoadingCard';

interface StationDetailsProps {
  stationId: string | null;
  close: () => void;
}

export default function StationDetails({ stationId, close }: StationDetailsProps) {
  if (!stationId) return null;

  const { station, readings = [], isLoading, error } = useStationDetails(stationId);
  const [selectedReadingId, setSelectedReadingId] = useState<string | null>(null);

  if (isLoading) return <LoadingCard />;
  if (error || !station) return <ErrorCard error={error} />;

  const latest = readings.at(-1);
  const active = readings.find(r => r.id === selectedReadingId) ?? latest;
  if (!active) return null;

  const isViewingHistory = active.id !== latest?.id;

  return (
    <div className={`${styles.container} ${styles.floating} card`}>
      <StationHeader
        station={station}
        reading={active}
        isViewingHistory={isViewingHistory}
        close={close}
      />

      <CurrentReading reading={active} />

      <ReadingHistory
        readings={readings}
        selectedId={selectedReadingId}
        onSelect={setSelectedReadingId}
        isViewingHistory={isViewingHistory}
      />
    </div>
  );
}
