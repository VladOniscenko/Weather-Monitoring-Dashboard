import { CloudRain, Snowflake } from 'lucide-react';
import { type WeatherReadingDto } from '@/client';
import styles from './StationDetails.module.css';
import { formatDate, formatTime } from '@/helpers/date';

interface ReadingRowProps {
    reading: WeatherReadingDto;
    isSelected: boolean;
    onSelect: (id: string | undefined) => void;
}

export default function ReadingRow({
    reading,
    isSelected,
    onSelect,
}: ReadingRowProps) {
    const hasRain = (reading.rain ?? 0) > 0;
    const hasSnow = (reading.snow ?? 0) > 0;

    return (
        <div
            className={`${styles.readingRow} ${
                isSelected ? styles.selectedRow : ''
            }`}
            onClick={() => onSelect(reading.id)}
        >
            <div className="flex flex-col">
                <span className="text-[10px] font-bold text-txt-muted">
                    {formatDate(reading.capturedAt)}
                </span>
                <span className="text-xs font-black text-txt">
                    {formatTime(reading.capturedAt)}
                </span>
            </div>

            <div className="flex-1 px-4 border-l border-border-line ml-4">
                <div className="flex items-center gap-2">
                    <span className="text-lg font-black text-txt">
                        {(reading.temperature ?? 0).toFixed(1)}°
                    </span>
                    <div className="flex gap-2">
                        {hasRain && (
                            <CloudRain size={12} className="text-blue-400" />
                        )}
                        {hasSnow && (
                            <Snowflake size={12} className="text-blue-200" />
                        )}
                    </div>
                </div>
            </div>

            <div className="text-[10px] font-bold text-brand uppercase border border-brand px-2 py-0.5 rounded">
                {reading.mainCondition}
            </div>
        </div>
    );
}
