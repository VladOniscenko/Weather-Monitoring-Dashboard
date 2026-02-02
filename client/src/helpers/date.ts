export const formatTime = (date?: string) =>
    date
        ? new Date(date).toLocaleTimeString([], {
              hour: '2-digit',
              minute: '2-digit',
          })
        : 'N/A';

export const formatDate = (date?: string) =>
    date
        ? new Date(date).toLocaleDateString([], {
              day: '2-digit',
              month: 'short',
          })
        : '';
