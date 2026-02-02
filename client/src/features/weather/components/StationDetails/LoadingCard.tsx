import { Loader2 } from 'lucide-react';
import styles from './StationDetails.module.css';

export default function LoadingCard() {
    return (
        <div
            className={`${styles.container} ${styles.floating} card flex flex-col items-center justify-center p-8`}
        >
            <Loader2 className="h-8 w-8 animate-spin text-brand mt-3" />
            <p className="mt-2 text-sm text-txt-muted">Loading Station...</p>
        </div>
    );
}
