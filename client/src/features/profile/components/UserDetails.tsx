import { useProfile } from '@/features/profile/hooks/useProfile';

const UserDetails = () => {
    const { user, isLoading } = useProfile();

    if (isLoading) {
        return <div>Loading user details...</div>;
    }

    if (!user) {
        return <div>User not found or not authorized.</div>;
    }

    return (
        <div className="user-details border rounded p-4 m-3 shadow-sm">
            <h2 className="text-xl font-semibold mb-2">
                {user.name ?? 'Unnamed User'}
            </h2>
            <p>
                <strong>Email:</strong> {user.email ?? 'N/A'}
            </p>
            <p>
                <strong>Role:</strong> {user.role ?? 'Unknown'}
            </p>
            <p>
                <strong>Joined:</strong>{' '}
                {user.createdAt
                    ? new Date(user.createdAt).toLocaleDateString()
                    : 'Unknown'}
            </p>
        </div>
    );
};

export default UserDetails;
