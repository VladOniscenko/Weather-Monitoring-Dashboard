import styles from './StationDetails.module.css';

interface ErrorCardProps {
    error: string | null;
}

export default function ErrorCard({ error }: ErrorCardProps) {
    return (
        <div
            className={`${styles.container} ${styles.floating} card p-6 text-center text-red-500`}
        >
            <p className="mt-2 text-sm text-txt-muted">
                {error || 'Station not found!'}
            </p>
        </div>
    );
}
