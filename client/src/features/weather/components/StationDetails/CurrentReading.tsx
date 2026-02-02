import {
    Wind,
    Gauge,
    CloudRain,
    Snowflake,
    Navigation,
    ArrowDown,
    ArrowUp,
    Mountain,
} from 'lucide-react';
import styles from './StationDetails.module.css';
import { type WeatherReadingDto } from '@/client';
import StatBox from './StatBox';

interface CurrentReadingProps {
    reading: WeatherReadingDto;
}

export default function CurrentReading({ reading }: CurrentReadingProps) {
    const rain = reading.rain ?? 0;
    const snow = reading.snow ?? 0;

    const temperature = (reading.temperature ?? 0).toFixed(1);
    const maxTemp = (reading.maxTemp ?? 0).toFixed(1);
    const minTemp = (reading.minTemp ?? 0).toFixed(1);

    const hasRain = rain > 0;
    const hasSnow = snow > 0;

    return (
        <div className="mt-6">
            <div className="mx-3 flex items-center justify-between mb-6">
                <div className="flex items-center gap-4">
                    <div className={styles.iconWrapper}>
                        <img
                            src={`https://openweathermap.org/img/wn/${reading.icon}@2x.png`}
                            alt="Weather"
                            className="w-16 h-16"
                        />
                    </div>
                    <div>
                        <span className="text-5xl font-black text-txt tracking-tighter">
                            {temperature}°
                        </span>
                        <p className="text-sm font-bold text-brand uppercase tracking-widest">
                            {reading.description}
                        </p>
                    </div>
                </div>

                <div className="text-right">
                    <div className="flex items-center gap-1 text-xs text-txt-muted mb-1">
                        <ArrowUp size={12} className="text-red-400" />
                        <span>Max: {maxTemp}°</span>
                    </div>
                    <div className="flex items-center gap-1 text-xs text-txt-muted">
                        <ArrowDown size={12} className="text-blue-400" />
                        <span>Min: {minTemp}°</span>
                    </div>
                </div>
            </div>

            <div className={styles.statsGrid}>
                <div className={styles.statGroup}>
                    <StatBox
                        icon={<Gauge size={14} className="text-brand" />}
                        value={
                            <>
                                {reading.pressure} <small>hPa</small>
                            </>
                        }
                        label="Sea Level"
                    />

                    <StatBox
                        icon={<Mountain size={14} className="text-brand" />}
                        value={
                            <>
                                {reading.groundLevel} <small>hPa</small>
                            </>
                        }
                        label="Ground"
                    />
                </div>

                <div className={styles.statGroup}>
                    <StatBox
                        icon={<Wind size={14} className="text-brand" />}
                        value={
                            <>
                                {reading.windSpeed} <small>m/s</small>
                            </>
                        }
                        label="Speed"
                    />

                    <StatBox
                        icon={
                            <Navigation
                                size={14}
                                className="text-brand"
                                style={{
                                    transform: `rotate(${reading.windDeg}deg)`,
                                }}
                            />
                        }
                        value={<>{reading.windDeg}°</>}
                        label="Direction"
                    />
                </div>

                <div className={styles.statGroup}>
                    <StatBox
                        icon={
                            <CloudRain
                                size={14}
                                className={
                                    hasRain
                                        ? 'text-blue-400'
                                        : 'text-txt-muted'
                                }
                            />
                        }
                        value={
                            <>
                                {rain} <small>mm</small>
                            </>
                        }
                        label="Rain"
                    />

                    <StatBox
                        icon={
                            <Snowflake
                                size={14}
                                className={
                                    hasSnow
                                        ? 'text-blue-200'
                                        : 'text-txt-muted'
                                }
                            />
                        }
                        value={
                            <>
                                {snow} <small>mm</small>
                            </>
                        }
                        label="Snow"
                    />
                </div>
            </div>
        </div>
    );
}
