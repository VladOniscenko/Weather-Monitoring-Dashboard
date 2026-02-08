import { useProfile } from '@/features/profile/hooks/useProfile';

function getInitials(name?: string | null) {
    if (!name) return '?';
    return name
        .split(' ')
        .map((p) => p[0])
        .slice(0, 2)
        .join('')
        .toUpperCase();
}

const UserDetails = () => {
    const { user, isLoading } = useProfile();

    if (isLoading) {
        return <div className="card">Loading user details…</div>;
    }

    if (!user) {
        return <div className="error">User not found or not authorized.</div>;
    }

    return (
        <section className="card space-y-6">
            {/* Profile Header */}
            <header className="flex items-center gap-4">
                {/* Avatar */}
                <div
                    className="flex items-center justify-center text-lg font-bold"
                    style={{
                        width: 56,
                        height: 56,
                        borderRadius: '50%',
                        background: 'var(--accent)',
                        color: 'var(--accent-fg)',
                    }}
                >
                    {getInitials(user.name)}
                </div>

                <div>
                    <h2>{user.name ?? 'Unnamed User'}</h2>
                    <p>{user.role ?? 'User'}</p>
                </div>
            </header>

            {/* Divider */}
            <div
                style={{
                    height: 1,
                    background: 'var(--border)',
                    opacity: 0.6,
                }}
            />

            {/* Info Grid */}
            <div className="grid sm:grid-cols-2 gap-4 text-sm">
                <InfoItem label="Email" value={user.email ?? 'N/A'} />

                <InfoItem label="Role" value={user.role ?? 'Unknown'} />

                <InfoItem
                    label="Member since"
                    value={
                        user.createdAt
                            ? new Date(user.createdAt).toLocaleDateString()
                            : 'Unknown'
                    }
                />

                <InfoItem label="Status" value="Active" />
            </div>
        </section>
    );
};

function InfoItem({ label, value }: { label: string; value: React.ReactNode }) {
    return (
        <div
            style={{
                padding: '0.75rem 1rem',
                borderRadius: 'var(--radius-base)',
                border: '1px solid var(--border)',
                background:
                    'color-mix(in srgb, var(--bg-surface) 70%, transparent)',
            }}
        >
            <div style={{ fontSize: '0.75rem', opacity: 0.7 }}>{label}</div>
            <div style={{ fontWeight: 600 }}>{value}</div>
        </div>
    );
}

export default UserDetails;
