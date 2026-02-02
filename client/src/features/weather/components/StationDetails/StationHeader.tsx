import { MapPin, Clock } from 'lucide-react';
import styles from './StationDetails.module.css';
import { formatTime } from '@/helpers/date';
import {
    type WeatherStationDto,
    type WeatherReadingDto,
} from '@/client';

interface StationHeaderProps {
    station: WeatherStationDto;
    reading: WeatherReadingDto;
    isViewingHistory: boolean;
    close: () => void;
}

export default function StationHeader({
    station,
    reading,
    isViewingHistory,
    close,
}: StationHeaderProps) {
    return (
        <header className={styles.header}>
            <div className="flex justify-between items-start">
                <div>
                    <h2 className="text-xl font-bold text-txt flex items-center gap-2">
                        <MapPin size={18} className="text-brand" />
                        {station.name}
                    </h2>
                    <p className="text-xs text-txt-muted flex items-center gap-1 mt-1">
                        <Clock
                            size={12}
                            className={
                                isViewingHistory
                                    ? 'text-amber-500'
                                    : 'text-brand'
                            }
                        />
                        {isViewingHistory ? 'History:' : 'Last measured:'}
                        <span
                            className={`font-medium ${
                                isViewingHistory
                                    ? 'text-amber-500'
                                    : 'text-txt'
                            }`}
                        >
                            {formatTime(reading.capturedAt)}
                        </span>
                    </p>
                </div>
                <button onClick={close} className={styles.closeBtn}>
                    &times;
                </button>
            </div>
        </header>
    );
}
