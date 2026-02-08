import { useState } from 'react';
import { useWeatherStations } from '@/features/weather/hooks/useWeatherStations';
import { useAuth } from '@/hooks/useAuth';
import { useMemo } from 'react';

const UserStations = ({ refreshKey }: { refreshKey: number }) => {
    const [page, setPage] = useState<number>(0);
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
                        className="card p-4 flex flex-col justify-between hover:shadow-md transition"
                    >
                        {/* Header */}
                        <div className="flex items-center justify-between mb-2">
                            <h3 className="font-semibold">
                                {station.name ?? 'Unnamed Station'}
                            </h3>
                            <span
                                className="inline-block w-3 h-3 rounded-full"
                                style={{ background: 'var(--accent)' }}
                                title="Active station"
                            />
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
