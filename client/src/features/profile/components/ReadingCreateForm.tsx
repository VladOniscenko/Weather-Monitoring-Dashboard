'use client';

import { useState, useEffect } from 'react';
import {
    Dialog,
    DialogBackdrop,
    DialogPanel,
    DialogTitle,
} from '@headlessui/react';
import { toast } from 'react-toastify';
import { type WeatherReadingDto, WeatherReadingService } from '@/client';

interface StationReadingFormProps {
    stationId: string;
    onClose: () => void;
}

export default function StationReadingForm({
    stationId,
    onClose,
}: StationReadingFormProps) {
    const mainConditionOptions = [
        'Clear',
        'Clouds',
        'Drizzle',
        'Fog',
        'Haze',
        'Mist',
        'Rain',
        'Snow',
        'Thunderstorm',
    ];

    const openWeatherIcons = [
        '01d',
        '01n',
        '02d',
        '02n',
        '03d',
        '03n',
        '04d',
        '04n',
        '09d',
        '09n',
        '10d',
        '10n',
        '11d',
        '11n',
        '13d',
        '13n',
        '50d',
        '50n',
    ];

    const [form, setForm] = useState<WeatherReadingDto>({
        stationId,
        mainCondition: '',
        description: '',
        icon: '',
        temperature: undefined,
        feelsLike: undefined,
        minTemp: undefined,
        maxTemp: undefined,
        pressure: undefined,
        humidity: undefined,
        windSpeed: undefined,
        windDeg: undefined,
        cloudiness: undefined,
        visibility: undefined,
        rain: undefined,
        snow: undefined,
        capturedAt: new Date().toISOString(),
    });

    useEffect(() => {
        setForm((prev) => ({
            ...prev,
            stationId,
            mainCondition: '',
            icon: '',
            capturedAt: new Date().toISOString(),
        }));
    }, [stationId]);

    function update<K extends keyof WeatherReadingDto>(
        key: K,
        value: WeatherReadingDto[K]
    ) {
        setForm((prev) => ({ ...prev, [key]: value }));
    }

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        try {
            await WeatherReadingService.createReading(form);
            toast.success('Reading saved successfully!');
            onClose();
        } catch (err: any) {
            console.log(err);
            const message =
                err?.body?.Message || err?.message || 'Failed to save reading';

            toast.error(message);
        }
    }

    return (
        <Dialog open={true} onClose={onClose} className="relative z-10">
            <DialogBackdrop className="fixed inset-0 bg-black/40 backdrop-blur-sm" />

            <div className="fixed inset-0 flex items-center justify-center p-4">
                <DialogPanel className="w-full max-w-lg max-h-[90vh] overflow-y-auto">
                    <form onSubmit={handleSubmit} className="card space-y-6">
                        {/* Header */}
                        <header className="mb-4">
                            <DialogTitle
                                as="h2"
                                className="font-semibold text-xl"
                            >
                                Add Weather Reading
                            </DialogTitle>
                            <p className="text-sm opacity-70">
                                Record a new measurement for this station.
                            </p>
                        </header>

                        {/* Main Condition */}
                        <div>
                            <label>Main condition</label>
                            <select
                                required
                                value={form.mainCondition ?? ''}
                                onChange={(e) =>
                                    update('mainCondition', e.target.value)
                                }
                            >
                                <option value="">Select condition</option>
                                {mainConditionOptions.map((cond) => (
                                    <option key={cond} value={cond}>
                                        {cond}
                                    </option>
                                ))}
                            </select>
                        </div>

                        {/* Icon */}
                        <div className="flex flex-col gap-2">
                            <label>Weather Icon</label>
                            <div className="flex items-center gap-4">
                                <select
                                    value={form.icon ?? ''}
                                    onChange={(e) =>
                                        update('icon', e.target.value)
                                    }
                                >
                                    <option value="">Select icon</option>
                                    {openWeatherIcons.map((icon) => (
                                        <option key={icon} value={icon}>
                                            {icon}
                                        </option>
                                    ))}
                                </select>

                                {/* Show selected icon */}
                                {form.icon && (
                                    <img
                                        src={`https://openweathermap.org/img/wn/${form.icon}.png`}
                                        alt="Selected weather icon"
                                        className="w-10 h-10"
                                    />
                                )}
                            </div>
                        </div>

                        {/* Description */}
                        <div>
                            <label>Description</label>
                            <input
                                value={form.description ?? ''}
                                onChange={(e) =>
                                    update('description', e.target.value)
                                }
                                placeholder="Optional description"
                            />
                        </div>

                        {/* Temperatures */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Temperature (°C)</label>
                                <input
                                    type="number"
                                    step="any"
                                    value={form.temperature ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'temperature',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                            <div>
                                <label>Feels Like (°C)</label>
                                <input
                                    type="number"
                                    step="any"
                                    value={form.feelsLike ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'feelsLike',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                        </div>

                        {/* Min/Max */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Min Temp (°C)</label>
                                <input
                                    type="number"
                                    value={form.minTemp ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'minTemp',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                            <div>
                                <label>Max Temp (°C)</label>
                                <input
                                    type="number"
                                    value={form.maxTemp ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'maxTemp',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                        </div>

                        {/* Pressure / Humidity */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Pressure (hPa)</label>
                                <input
                                    type="number"
                                    value={form.pressure ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'pressure',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                            <div>
                                <label>Humidity (%)</label>
                                <input
                                    type="number"
                                    value={form.humidity ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'humidity',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                        </div>

                        {/* Wind */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Wind Speed (m/s)</label>
                                <input
                                    type="number"
                                    value={form.windSpeed ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'windSpeed',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                            <div>
                                <label>Wind Direction (°)</label>
                                <input
                                    type="number"
                                    value={form.windDeg ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'windDeg',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                        </div>

                        {/* Cloudiness / Visibility */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Cloudiness (%)</label>
                                <input
                                    type="number"
                                    value={form.cloudiness ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'cloudiness',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                            <div>
                                <label>Visibility (m)</label>
                                <input
                                    type="number"
                                    value={form.visibility ?? ''}
                                    onChange={(e) =>
                                        update(
                                            'visibility',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>
                        </div>

                        {/* Rain / Snow */}
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label>Rain (mm)</label>
                                <input
                                    type="number"
                                    step="any"
                                    value={form.rain ?? ''}
                                    onChange={(e) =>
                                        update('rain', Number(e.target.value))
                                    }
                                />
                            </div>
                            <div>
                                <label>Snow (mm)</label>
                                <input
                                    type="number"
                                    step="any"
                                    value={form.snow ?? ''}
                                    onChange={(e) =>
                                        update('snow', Number(e.target.value))
                                    }
                                />
                            </div>
                        </div>

                        {/* Timestamp */}
                        <div>
                            <label>Captured at</label>
                            <input
                                type="datetime-local"
                                value={form.capturedAt?.slice(0, 16)}
                                onChange={(e) =>
                                    update(
                                        'capturedAt',
                                        new Date(e.target.value).toISOString()
                                    )
                                }
                            />
                        </div>

                        {/* Actions */}
                        <footer className="flex justify-end gap-3 pt-2">
                            <button type="button" onClick={onClose}>
                                Cancel
                            </button>
                            <button type="submit">Save Reading</button>
                        </footer>
                    </form>
                </DialogPanel>
            </div>
        </Dialog>
    );
}
