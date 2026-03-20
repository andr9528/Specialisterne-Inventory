import { useEffect, useState } from "react";

const tailwindScreens = {
    sm: "640px",
    md: "768px",
    lg: "1024px",
    xl: "1280px",
    "2xl": "1536px",
};

type TailwindScreensTypes = keyof typeof tailwindScreens;

/**
 * TailwindMediaQuery hook to check if the screen width is within one of the default tailwind breakpoints.
 * @param screenKey - The tailwind breakpoint
 * @param listener - Optional listener function to execute if the screen width matches the provide breakpoint
 * @returns returns true if the document currently matches the provided tailwind breakpoint, or false if not
 */
const useTailwindMediaQuery = (screenKey: TailwindScreensTypes, listener?: () => void) => {
    const minWidth = tailwindScreens[screenKey];
    const query = `(min-width: ${minWidth})`;

    const [matches, setMatches] = useState<boolean>(false);

    useEffect(() => {
        const media = window.matchMedia(query);
        setMatches(media.matches);

        const onChange = (e: MediaQueryListEvent) => {
            setMatches(e.matches);

            if (listener && e.matches) {
                listener();
            }
        }

        media.addEventListener("change", onChange);

        return () => media.removeEventListener("change", onChange);
    }, [query, listener]);

    return matches
}

export default useTailwindMediaQuery;