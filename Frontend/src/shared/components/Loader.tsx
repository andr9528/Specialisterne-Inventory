import { LoaderIcon } from "lucide-react";

const Loader = ({ message = "Loading..." }: { message?: string }) => {
    return (
        <div className="flex flex-col items-center justify-center h-full min-h-50">
            <LoaderIcon className="text-primary w-12 h-12 mb-4 animate-spin" style={{ animation: "spin 2s linear infinite" }} />
            <p className="text-gray-500 text-lg pl-2">{message}</p>
        </div>
    );
};

export default Loader;