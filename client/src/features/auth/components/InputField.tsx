interface InputFieldProps {
    type: 'email' | 'password' | 'text';
    placeholder?: string;
    value: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const InputField = ({
    type,
    placeholder,
    value,
    onChange,
}: InputFieldProps) => {
    return (
        <div className="mb-4">
            <input
                type={type}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
            />
        </div>
    );
};

export default InputField;
    