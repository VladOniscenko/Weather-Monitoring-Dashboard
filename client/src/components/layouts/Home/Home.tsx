'use client';

import { Link } from 'react-router-dom';

export default function HomePage() {
    return (
        <main className="relative flex flex-col items-center bg-main text-primary min-h-screen overflow-x-hidden">
            {/* Hero Section */}
            <section className="relative w-full flex flex-col items-center justify-center text-center pt-24 pb-48 px-4 sm:pt-32 sm:pb-64">
                <h1 className="text-5xl sm:text-6xl font-bold mb-6">
                    🌍 Weather Station Network
                </h1>
                <p className="text-secondary max-w-2xl mb-8 text-lg sm:text-xl">
                    Discover, share, and monitor environmental data from
                    stations worldwide. Join the global community of weather
                    enthusiasts today.
                </p>

                {/* Call-to-Action Buttons */}
                <div className="flex flex-col sm:flex-row gap-4 justify-center z-10">
                    <Link to="/map" className="btn">
                        Explore Stations
                    </Link>
                    <Link to="/profile" className="btn">
                        Add Your Station
                    </Link>
                </div>

                {/* Map Hero */}
                <div className="mt-16 relative w-full max-w-6xl h-[28rem] sm:h-[36rem] flex justify-center">
                    <div className="absolute inset-0 rounded-xl overflow-hidden shadow-xl">
                        <img
                            src="/map_view.png"
                            alt="Global weather station map"
                            className="w-full h-full object-cover"
                        />
                    </div>

                    {/* Floating Station Card Example */}
                    <div className="absolute bottom-12 left-1/2 transform -translate-x-1/2 bg-surface p-6 rounded-xl shadow-xl max-w-md w-full text-left">
                        <h2 className="text-2xl font-bold mb-2">
                            Realtime Data
                        </h2>
                        <p className="text-secondary">
                            Click any station to view live temperature,
                            humidity, and wind readings directly on the map.
                        </p>
                    </div>
                </div>
            </section>

            {/* Features Section */}
            <section className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-24 max-w-5xl w-full px-4">
                <div className="card hover:shadow-lg transition">
                    <h3 className="text-xl font-semibold mb-2">
                        🔹 Real-Time Measurements
                    </h3>
                    <p className="text-secondary">
                        See the latest temperature, humidity, wind, and pressure
                        readings from every station worldwide.
                    </p>
                </div>

                <div className="card hover:shadow-lg transition">
                    <h3 className="text-xl font-semibold mb-2">
                        🔹 Community Network
                    </h3>
                    <p className="text-secondary">
                        Share your station, explore others, and contribute to a
                        growing global dataset.
                    </p>
                </div>

                <div className="card hover:shadow-lg transition">
                    <h3 className="text-xl font-semibold mb-2">
                        🔹 Custom Stations
                    </h3>
                    <p className="text-secondary">
                        Add your own station in minutes and track your local
                        environment with high fidelity.
                    </p>
                </div>
            </section>

            {/* Call-to-Action Section */}
            <section className="text-center mb-24 px-4">
                <h2 className="text-3xl font-bold mb-4">
                    Start Exploring Today
                </h2>
                <p className="text-secondary mb-6">
                    Whether you want to explore stations or contribute your own,
                    it’s easy to join the network.
                </p>
                <Link to="/profile" className="btn">
                    Create Your Station
                </Link>
            </section>

            {/* Footer */}
            <footer className="text-center text-sm text-secondary mb-6">
                Developed by Vlad —{' '}
                <em>
                    Bridging the gap between hardware measurements and beautiful
                    data.
                </em>
            </footer>
        </main>
    );
}
