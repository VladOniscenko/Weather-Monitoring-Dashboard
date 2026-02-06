export const validateEmail = (email: string): string | null => {
    const trimmed = email.trim();

    if (!trimmed) {
        return 'Email is required';
    }

    const emailRegex =
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailRegex.test(trimmed)) {
        return 'Please enter a valid email address';
    }

    return null;
};


export const validateFullName = (name: string): string | null => {
    const trimmed = name.trim();
    if (!trimmed) {
        return 'Full name is required';
    }

    const parts = trimmed.split(/\s+/);
    if (parts.length < 2) {
        return 'Please enter your full name';
    }

    const validPart = /^[A-Za-zÀ-ÖØ-öø-ÿ'’-]{2,}$/;
    for (const part of parts) {
        if (!validPart.test(part)) {
            return 'Name contains invalid characters';
        }
    }

    return null;
};


export const validatePassword = (password: string): string | null => {
    if (!password) {
        return 'Password is required';
    }

    if (password.length < 8) {
        return 'Password must be at least 8 characters';
    }

    if (/\s/.test(password)) {
        return 'Password must not contain spaces';
    }

    if (!/[A-Za-z]/.test(password)) {
        return 'Password must contain at least one letter';
    }

    if (!/\d/.test(password)) {
        return 'Password must contain at least one number';
    }

    return null;
};
