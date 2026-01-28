import styles from "./StationDetails.module.css";
import { useWeatherStation } from "@/features/weather/hooks/useWeatherStation";
import { useWeatherReadings } from "@/features/weather/hooks/useWeatherReading";
import { Loader2 } from "lucide-react";

interface StationInfoProps {
  isOpen: boolean;
  stationId: string;
  close: () => void;
}

const StationDetails = ({ isOpen, stationId, close }: StationInfoProps) => {
  const { data: station, loading: stationLoading } = useWeatherStation(stationId);
  const { data: readings, loading: readingsLoading } = useWeatherReadings(station?.id);

  if (!isOpen) return null;

  // Group readings by day
  const readingsByDay: Record<string, typeof readings> = {};
  readings?.forEach((r) => {
    const date = new Date(r.capturedAt);
    const day = date.toLocaleDateString(); // e.g. "1/28/2026"
    if (!readingsByDay[day]) readingsByDay[day] = [];
    readingsByDay[day].push(r);
  });

  // Sort days descending (latest day first)
  const sortedDays = Object.keys(readingsByDay).sort((a, b) => new Date(b).getTime() - new Date(a).getTime());

  return (
    <div className={`${styles.container} card p-4 space-y-4`}>
      {/* Station loader */}
      {stationLoading && (
        <div className="flex justify-center py-6">
          <Loader2 className="h-6 w-6 animate-spin" />
        </div>
      )}

      {station && (
        <>
          <div className="flex justify-between items-center">
            <h3 className="font-bold text-lg">{station.name}</h3>
            <button onClick={close} className="text-gray-400 hover:text-gray-600">
              Close
            </button>
          </div>

          <div className="text-gray-600 space-y-1 text-sm">
            <p>ID: {station.id}</p>
            <p>Latitude: {station.latitude}</p>
            <p>Longitude: {station.longitude}</p>
            <p>Last Synced: {new Date(station.lastSyncedAt).toLocaleString()}</p>
          </div>

          {/* Weather readings */}
          <div className="mt-4 max-h-72 overflow-y-auto space-y-2">
            {readingsLoading && (
              <div className="flex justify-center">
                <Loader2 className="h-5 w-5 animate-spin text-gray-400" />
              </div>
            )}

            {!readingsLoading && readings.length === 0 && (
              <p className="text-sm text-gray-500">No weather readings available.</p>
            )}

            {!readingsLoading &&
              sortedDays.map((day) => (
                <div key={day} className="space-y-1">
                  <p className="font-semibold text-gray-700">{day}</p>
                  {readingsByDay[day].map((reading) => {
                    const hour = new Date(reading.capturedAt).getHours();
                    const isDay = hour >= 6 && hour < 18; // day between 6:00 and 18:00
                    return (
                      <div
                        key={reading.id}
                        className={`flex justify-between items-center border-b border-gray-200 py-1 text-sm ${
                          isDay ? "bg-yellow-50" : "bg-gray-50"
                        }`}
                      >
                        <div className="flex flex-col">
                          <span className="font-medium">{reading.mainCondition}</span>
                          <span className="text-gray-500">{reading.description}</span>
                        </div>
                        <div className="text-right text-gray-500">
                          <span>
                            {reading.temperature.toFixed(1)}°C / {reading.feelsLike.toFixed(1)}°C
                          </span>
                          <br />
                          <span>💧 {reading.humidity}%</span>
                        </div>
                      </div>
                    );
                  })}
                </div>
              ))}
          </div>
        </>
      )}
    </div>
  );
};

export default StationDetails;
