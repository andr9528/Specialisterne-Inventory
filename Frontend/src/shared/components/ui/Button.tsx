import { LoaderIcon, type LucideProps } from "lucide-react";
import type { ButtonHTMLAttributes } from "react";

export type ButtonVariant = "primary" | "ghost" | "tag" | "outline";

interface ButtoneProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: ButtonVariant;
    isLoading?: boolean;
    icon?: React.ForwardRefExoticComponent<Omit<LucideProps, "ref">> | null;
}

const Button: React.FC<ButtoneProps> = ({
    children,
    variant = "primary",
    className = "",
    icon: Icon,
    isLoading = false,
    ...props
}) => {

    const baseStyle = "hover:cursor-pointer outline-none disabled:pointer-events-none disabled:opacity-50 transition-colors flex";

    let variantStyle = "";

    switch (variant) {
        case "primary":
            variantStyle = "bg-blue-600 hover:bg-blue-700 rounded-lg text-white p-2 px-3";
            break;
        case "outline":
            variantStyle = "rounded-lg p-1 px-3 border border-gray-300 hover:bg-gray-100";
            break;
        case "ghost":
            variantStyle = "border-none hover:bg-gray-300/80 rounded-lg p-2";
            break;
        case "tag":
            variantStyle = "bg-gray-200 text-gray-700 hover:bg-gray-300 rounded-2xl px-3 py-0.5 text-sm font-medium";
            break;
    }

    const iconStyle = "size-4 mt-1 mr-3";

    return (
        <button
            className={`${baseStyle} ${variantStyle}  ${className}`}
            {...props}
        >
            {Icon &&
                <span>
                    <Icon className={iconStyle} />
                </span>
            }
            {isLoading ? <LoaderIcon className="animate-spin" style={{ animation: "spin 2.5s linear infinite" }} /> : children}
        </button>
    )
}

export default Button;