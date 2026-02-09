import { useState } from 'react';
import { useWeatherStations } from '@/features/weather/hooks/useWeatherStations';
import { useAuth } from '@/hooks/useAuth';
import { useMemo } from 'react';
import { toast } from 'react-toastify';
import { WeatherStationsService } from '@/client';
import StationReadingForm from './ReadingCreateForm';

const UserStations = ({
    refreshKey,
    refreshStations,
}: {
    refreshKey: number;
    refreshStations: () => void;
}) => {
    const [page, setPage] = useState<number>(0);
    const [openStationId, setOpenStationId] = useState<string | null>(null);

    const { user } = useAuth();

    const params = useMemo(
        () => ({
            userId: user?.id,
            page,
            pageSize: 16,
            refreshKey,
        }),
        [user?.id, page, refreshKey]
    );

    const { data: stations, loading, error } = useWeatherStations(params);

    function handlePageChange(next: number) {
        if (next < 0) return;
        if (stations?.totalPages && next >= stations.totalPages) return;
        setPage(next);
    }

    if (loading) {
        return <div className="card">Loading stations…</div>;
    }

    if (error) {
        return (
            <div className="error">
                <strong>Error loading stations</strong>
                <div>{typeof error === 'string' ? error : 'Unknown error'}</div>
            </div>
        );
    }

    if (!stations?.items || stations.items.length === 0) {
        return <div className="card">No stations found.</div>;
    }

    async function handleDelete(id: string) {
        try {
            await WeatherStationsService.deleteStation(id);

            toast.success('Station deleted');
            refreshStations();
        } catch (err) {
            const message =
                err instanceof Error ? err.message : 'Failed to delete station';
            toast.error(message);
        }
    }

    return (
        <section className="space-y-6">
            {/* Pagination */}
            <div className="card flex flex-col sm:flex-row justify-between items-center gap-4">
                <div className="flex gap-2">
                    <button
                        disabled={page < 1}
                        onClick={() => handlePageChange(page - 1)}
                    >
                        Previous
                    </button>
                    <button
                        disabled={page >= (stations.totalPages ?? 1) - 1}
                        onClick={() => handlePageChange(page + 1)}
                    >
                        Next
                    </button>
                </div>
                <div>
                    Page {page + 1} of {stations.totalPages ?? 1}
                </div>
            </div>

            {/* Total Stations */}
            <div className="text-center text-sm opacity-70">
                Total Stations: {stations.totalItems ?? stations.items.length}
            </div>

            {/* Station Grid */}
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
                {stations.items.map((station) => (
                    <article
                        key={station.id}
                        onClick={() =>
                            station.id && setOpenStationId(station.id)
                        }
                        className="card p-4 flex flex-col justify-between hover:shadow-md transition cursor-pointer"
                    >
                        {/* Header */}
                        <div className="flex items-center justify-between mb-2">
                            <h3 className="font-semibold">
                                {station.name ?? 'Unnamed Station'}
                            </h3>

                            {station.id && (
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        if (!station.id) return;
                                        const ok = window.confirm(
                                            `Delete "${station.name ?? 'this station'}"?`
                                        );
                                        if (!ok) return;

                                        handleDelete(station.id);
                                    }}
                                    className="error"
                                >
                                    Delete
                                </button>
                            )}
                        </div>

                        {/* Metadata */}
                        <div className="grid grid-cols-1 gap-1 text-sm">
                            <InfoBadge
                                label="Latitude"
                                value={station.latitude ?? 'N/A'}
                            />
                            <InfoBadge
                                label="Longitude"
                                value={station.longitude ?? 'N/A'}
                            />
                        </div>
                    </article>
                ))}
            </div>

            {openStationId && (
                <StationReadingForm
                    stationId={openStationId}
                    onClose={() => setOpenStationId(null)}
                />
            )}
        </section>
    );
};

function InfoBadge({
    label,
    value,
}: {
    label: string;
    value: React.ReactNode;
}) {
    return (
        <div
            style={{
                padding: '0.35rem 0.75rem',
                borderRadius: 'var(--radius-base)',
                background:
                    'color-mix(in srgb, var(--bg-surface) 70%, transparent)',
                border: '1px solid var(--border)',
                display: 'inline-block',
            }}
        >
            <div style={{ fontSize: '0.7rem', opacity: 0.7 }}>{label}</div>
            <div style={{ fontWeight: 600 }}>{value}</div>
        </div>
    );
}

export default UserStations;
