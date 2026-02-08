// helper to get initials
export const getInitials = (name?: string) => {
  if (!name) return 'U';
  const parts = name.trim().split(' ');
  return (parts[0][0] + (parts[1]?.[0] ?? '')).toUpperCase();
};