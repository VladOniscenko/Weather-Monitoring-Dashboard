import styles from './StationDetails.module.css';

interface StatBoxProps {
    icon: React.ReactNode;
    value: React.ReactNode;
    label: string;
}

export default function StatBox({ icon, value, label }: StatBoxProps) {
    return (
        <div className={styles.statBox}>
            {icon}
            <span>{value}</span>
            <label>{label}</label>
        </div>
    );
}
