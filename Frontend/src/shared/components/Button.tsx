import { LoaderIcon } from "lucide-react";
import type { ButtonHTMLAttributes } from "react";

export type ButtonVariant = "primary" | "ghost" | "tag";
interface ButtoneProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: ButtonVariant;
    size?: "small" | "medium" | "large";
    isLoading?: boolean;
}

const Button: React.FC<ButtoneProps> = ({
    children,
    variant = "primary",
    size = "medium",
    className = "",
    isLoading = false,
    ...props
}) => {

    const baseStyle = "hover:cursor-pointer outline-none disabled:pointer-events-none disabled:opacity-50 transition-colors";

    let variantStyle = "";

    switch (variant) {
        case "primary":
            variantStyle = "";
            break;
        case "ghost":
            variantStyle = "border-none";
            break;
        case "tag":
            variantStyle = "bg-gray-200 text-gray-700 hover:bg-gray-300 rounded-2xl px-3 py-0.5 text-sm font-medium";
            break;
    }

    return (
        <button
            className={`${baseStyle} ${variantStyle}  ${className}`}
            {...props}
        >
            {isLoading ? <LoaderIcon className="animate-spin" style={{ animation: "spin 2.5s linear infinite" }} /> : children}
        </button>
    )
}

export default Button;