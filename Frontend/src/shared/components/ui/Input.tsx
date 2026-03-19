import { type InputHTMLAttributes } from "react";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
}

const Input: React.FC<InputProps> = ({ ...props }) => {

    return (
        <div className="relative">
            <input
                className="min-h-8 py-1 px-2 bg-input-background rounded-lg border border-gray-300 outline-none focus-visible:ring-[1px] focus-visible:border-ring focus-visible:ring-ring w-full"
                {...props}
            />
        </div>
    )
}

export default Input;