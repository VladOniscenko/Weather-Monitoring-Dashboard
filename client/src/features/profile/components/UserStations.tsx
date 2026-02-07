import { useState } from 'react';
import { useWeatherStations } from '@/features/weather/hooks/useWeatherStations';

const UserStations = () => {
    const [page, setPage] = useState<number>(0);
    const {
        data: stations,
        loading,
        error,
    } = useWeatherStations({
        page: page,
        pageSize: 16,
    });

    const handlePageChange = (page: number) => {
        if (page < 0 || (stations?.totalPages && page > stations.totalPages))
            return;
        setPage(page);
    };

    if (loading) {
        return <div>Loading stations...</div>;
    }

    if (error) {
        return (
            <div className="text-red-600">
                Error loading stations:{' '}
                {typeof error === 'string' ? error : 'Unknown error'}
            </div>
        );
    }

    if (!stations?.items || stations.items.length === 0) {
        return <div>No stations found.</div>;
    }

    return (
        <div className="user-stations p-3">
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mt-3">
                {stations.items.map((station) => (
                    <div
                        key={station.id}
                        className="border rounded p-3 shadow-sm hover:shadow-md transition"
                    >
                        <h3 className="font-semibold text-lg">
                            {station.name ?? 'Unnamed Station'}
                        </h3>
                        <p>
                            <strong>Latitude:</strong>{' '}
                            {station.latitude ?? 'N/A'}
                        </p>
                        <p>
                            <strong>Longitude:</strong>{' '}
                            {station.longitude ?? 'N/A'}
                        </p>
                    </div>
                ))}
            </div>

            {/* Pagination Controls */}
            <div className="pagination flex justify-center items-center gap-2 mt-4">
                <button
                    className="px-3 py-1 border rounded disabled:opacity-50"
                    disabled={page <= 1}
                    onClick={() => handlePageChange(page - 1)}
                >
                    Previous
                </button>

                <span>
                    Page {page} of {stations.totalPages ?? 1}
                </span>

                <button
                    className="px-3 py-1 border rounded disabled:opacity-50"
                    disabled={page >= (stations.totalPages ?? 1)}
                    onClick={() => handlePageChange(page + 1)}
                >
                    Next
                </button>
            </div>

            {/* Total Stations Info */}
            <div className="text-center text-sm text-gray-500 mt-1">
                Total Stations: {stations.totalItems ?? stations.items.length}
            </div>
        </div>
    );
};

export default UserStations;
