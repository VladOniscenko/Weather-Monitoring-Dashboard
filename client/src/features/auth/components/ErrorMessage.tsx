interface ErrorMessageProps {
    error: string | null;
}

export default function ErrorCard({ error }: ErrorMessageProps) {
    if (!error) return;

    return <div className='error'>{error}</div>;
}
