import { useEffect } from "react";

/**
 * Tracks whether the user clicks outside of the provided refs
 * @param ref ref or array of refs to track
 * @param onClickOutside callback which is called if the user has clicked outside of all provided refs
 */
const useClickOutside = (
    ref: React.RefObject<HTMLElement | null> | React.RefObject<HTMLElement | null>[],
    onClickOutside: () => void
) => {
    const refArray = Array.isArray(ref) ? ref : [ref];

    useEffect(() => {
        const handleClickOutside = (e: MouseEvent | TouchEvent) => {

            const isOutsideAllRefs: boolean = refArray.every((ref) => {
                // Ignore unmounted refs
                if (!ref.current) {
                    return true;
                }

                return !ref.current.contains(e?.target as Node);
            })

            if (isOutsideAllRefs) {
                onClickOutside();
            }
        }

        // Handles both clicks on desktop and mobile
        window.addEventListener("mousedown", handleClickOutside);
        window.addEventListener("touchstart", handleClickOutside);

        return () => {
            window.removeEventListener("mousedown", handleClickOutside);
            window.removeEventListener("touchstart", handleClickOutside);
        }

    }, [ref, onClickOutside]);
}

export default useClickOutside;