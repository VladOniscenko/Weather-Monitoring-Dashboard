import { useStations } from '../hooks/useStations';

export const StationList = () => {
  // The component is clean. It doesn't know about Axios or Generated code.
  const { data, isLoading } = useStations();

  if (isLoading) return <div>Loading...</div>;

  return (
    <div>
      stations
      {data?.map((station: any) => (
        <div key={station.id}>{station.name}</div>
      ))}
    </div>
  );
}