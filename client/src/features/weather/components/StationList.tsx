import { useStations } from '../hooks/useStations';
import { useState } from "react";
import { type WeatherStationDto } from "@/client";

export const StationList = () => {
  const [name, setName] = useState<string>();
  const { data: stations, isLoading } = useStations({ name: name });

  if (isLoading) return <div>Loading stations…</div>;
  if (!stations) return <div>No Stations found</div>;

  return (
    <>
      <div>
        <input value={name} onChange={(e) => setName(e.target.value)} className="border-gray-300 border my-3" />
      </div>

      <div className="flex flex-row">
        <table className="table-auto border border-gray-300 w-full text-left">
          <thead className="bg-gray-100">
            <tr>
              <th className="px-4 py-2 border-b border-gray-300">Name</th>
              <th className="px-4 py-2 border-b border-gray-300">Latitude</th>
              <th className="px-4 py-2 border-b border-gray-300">Longitude</th>
              <th className="px-4 py-2 border-b border-gray-300">Since At</th>
            </tr>
          </thead>
          <tbody>
            {stations.map((station: WeatherStationDto) => {
              const lastSynced = station.lastSyncedAt ? new Date(station.lastSyncedAt) : null;
              const minutesAgo = lastSynced ? Math.floor(
                (Date.now() - lastSynced.getTime()) / 60000
              ) : null;
              return (
                <tr key={station.id} className="hover:bg-gray-50">
                  <td className="px-4 py-2 border-b border-gray-300">{station.name}</td>
                  <td className="px-4 py-2 border-b border-gray-300">{station.latitude?.toFixed(4)}</td>
                  <td className="px-4 py-2 border-b border-gray-300">{station.longitude?.toFixed(4)}</td>
                  <td className="px-4 py-2 border-b border-gray-300">{minutesAgo !== null ? `${minutesAgo} minutes ago` : 'Never'}</td>
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>
    </>
  );
};
