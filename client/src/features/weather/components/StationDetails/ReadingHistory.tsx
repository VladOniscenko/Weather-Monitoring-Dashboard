import { type WeatherReadingDto } from '@/client';
import styles from './StationDetails.module.css';
import ReadingRow from './ReadingRow';
interface ReadingHistoryProps {
    readings: WeatherReadingDto[];
    selectedId: string | null;
    onSelect: (id: string | null) => void;
    isViewingHistory: boolean;
}

export default function ReadingHistory({
    readings,
    selectedId,
    onSelect,
    isViewingHistory,
}: ReadingHistoryProps) {
    return (
        <div className={styles.historySection}>
            <div className="flex justify-between items-center mb-4 px-6">
                <h3 className="text-[10px] font-black uppercase tracking-widest text-txt-muted">
                    Reading History
                </h3>
                {isViewingHistory && (
                    <button
                        onClick={() => onSelect(readings.at(-1)?.id ?? null)}
                        className="text-[10px] h-0 text-brand font-bold hover:underline"
                    >
                        Return to Live
                    </button>
                )}
            </div>

            <div className={styles.readingsList}>
                {[...readings].reverse().map((reading) => (
                    <ReadingRow
                        key={reading.id}
                        reading={reading}
                        isSelected={reading.id === selectedId}
                        onSelect={onSelect}
                    />
                ))}
            </div>
        </div>
    );
}
