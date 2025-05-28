import axios from "axios";
import { useState } from "react";
import "./index.css";

type TrackingInfo = {
	trackingNumber: string;
	status: string;
};

function App() {
	const [trackingNumber, setTrackingNumber] = useState("");
	const [result, setResult] = useState<TrackingInfo | null>(null);
	const [error, setError] = useState<string | null>(null);
	const [loading, setLoading] = useState(false);

	const handleSearch = async () => {
		if (!trackingNumber.trim()) return;

		setLoading(true);
		setResult(null);
		setError(null);

		try {
			const response = await axios.get<TrackingInfo>(
				`http://localhost:5216/api/tracking/${trackingNumber.trim()}`
			);
			setResult(response.data);
		} catch {
			setError("Tracking number not exists.");
		} finally {
			setLoading(false);
		}
	};

	return (
		<div className="min-h-screen bg-gradient-to-b from-blue-50 to-grey text-gray-800 font-sans">
			<header className="bg-white shadow p-4 flex items-center justify-between px-8">
				<div className="flex items-center gap-4">
					<div className="text-blue-600 text-xl font-bold">
						📦 ParcelTracker
					</div>
					<span className="text-gray-500">Ireland</span>
				</div>
			</header>

			{/* Main */}
			<main className="flex flex-col items-center justify-center mt-20 px-4">
				<div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md animate-fade-in">
					<h1 className="text-2xl font-semibold mb-4 text-center">
						Parcel Tracking
					</h1>

					<input
						type="text"
						value={trackingNumber}
						onChange={e => setTrackingNumber(e.target.value)}
						placeholder="Enter the package"
						className="w-full border border-gray-300 rounded px-4 py-2 mb-4 focus:outline-none focus:ring-2 focus:ring-blue-500 text-white"
					/>
					<button
						onClick={handleSearch}
						disabled={loading}
						className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition-all"
					>
						{loading ? "Searching..." : "Search"}
					</button>

					{result && (
						<div className="mt-6 bg-green-100 border border-green-400 rounded p-4 text-sm animate-slide-up">
							<p>
								<strong>Number:</strong> {result.trackingNumber}
							</p>
							<p>
								<strong>Status:</strong> {result.status}
							</p>
						</div>
					)}

					{error && (
						<p className="mt-4 text-red-600 text-center animate-fade-in">
							{error}
						</p>
					)}
				</div>
			</main>
		</div>
	);
}

export default App;
