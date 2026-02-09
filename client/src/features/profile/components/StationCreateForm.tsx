'use client';

import { useState } from 'react';
import {
    Dialog,
    DialogBackdrop,
    DialogPanel,
    DialogTitle,
} from '@headlessui/react';
import { useCities } from '@/features/gelocation/hooks/useCities';
import {
    type CreateWeatherStationRequest,
    WeatherStationsService,
} from '@/client';
import { toast } from 'react-toastify';

export default function StationCreateForm({
    refreshStations,
}: {
    refreshStations: () => void;
}) {
    const [open, setOpen] = useState<boolean>(false);
    const [form, setForm] = useState<CreateWeatherStationRequest>({
        name: '',
        latitude: undefined,
        longitude: undefined,
        cityId: undefined,
    });

    const [cityQuery, setCityQuery] = useState<string>();
    const { data: cities, loading } = useCities({ name: cityQuery });

    function update<K extends keyof CreateWeatherStationRequest>(
        key: K,
        value: CreateWeatherStationRequest[K]
    ) {
        setForm((prev) => ({ ...prev, [key]: value }));
    }

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        try {
            const response = await WeatherStationsService.createStation(form);
            if (!response.success) {
                throw new Error(response.message || "Could't create station");
            }
            toast.success('Saved successfully!');
            setTimeout(refreshStations, 1000);
        } catch (err) {
            const message =
                err instanceof Error ? err.message : 'Something went wrong!';

            toast.error(message);
        } finally {
            setOpen(false);
        }
    }

    return (
        <div className="w-full flex justify-end py-4">
            <button onClick={() => setOpen(true)}>Create station</button>

            <Dialog open={open} onClose={setOpen} className="relative z-10">
                <DialogBackdrop className="fixed inset-0 bg-black/40 backdrop-blur-sm" />

                <div className="fixed inset-0 flex items-center justify-center p-4">
                    <DialogPanel className="w-full max-w-lg">
                        <form
                            onSubmit={handleSubmit}
                            className="card space-y-6"
                        >
                            <header>
                                <DialogTitle as="h2">
                                    Create Weather Station
                                </DialogTitle>
                                <p>Register a new measurement location.</p>
                            </header>

                            {/* Station Name */}
                            <div>
                                <label>Station name</label>
                                <input
                                    required
                                    value={form.name ?? ''}
                                    onChange={(e) =>
                                        update('name', e.target.value)
                                    }
                                    placeholder="e.g. Downtown Station"
                                />
                            </div>

                            {/* Coordinates */}
                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label>Latitude</label>
                                    <input
                                        required
                                        type="number"
                                        step="any"
                                        value={form.latitude ?? ''}
                                        onChange={(e) =>
                                            update(
                                                'latitude',
                                                Number(e.target.value)
                                            )
                                        }
                                    />
                                </div>

                                <div>
                                    <label>Longitude</label>
                                    <input
                                        required
                                        type="number"
                                        step="any"
                                        value={form.longitude ?? ''}
                                        onChange={(e) =>
                                            update(
                                                'longitude',
                                                Number(e.target.value)
                                            )
                                        }
                                    />
                                </div>
                            </div>

                            {/* City Search */}
                            <div>
                                <label>Search city</label>
                                <input
                                    placeholder="Type to search..."
                                    onChange={(e) =>
                                        setCityQuery(e.target.value)
                                    }
                                />
                            </div>

                            {/* City Select */}
                            <div>
                                <label>City</label>
                                <select
                                    required
                                    value={form.cityId ?? ''}
                                    onChange={(e) =>
                                        update('cityId', e.target.value)
                                    }
                                >
                                    <option value="">Select city</option>
                                    {cities?.items?.map((city) => (
                                        <option key={city.id} value={city.id}>
                                            {city.name}
                                        </option>
                                    ))}
                                </select>

                                {loading && <p>Loading cities…</p>}
                            </div>

                            {/* Actions */}
                            <footer className="flex justify-end gap-3 pt-2">
                                <button
                                    type="button"
                                    onClick={() => setOpen(false)}
                                >
                                    Cancel
                                </button>

                                <button type="submit">Create station</button>
                            </footer>
                        </form>
                    </DialogPanel>
                </div>
            </Dialog>
        </div>
    );
}
