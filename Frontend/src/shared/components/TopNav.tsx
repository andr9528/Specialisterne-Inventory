import { NavLink, type NavLinkRenderProps } from "react-router-dom";
import { router } from "../../app/router";
import { Menu, Package, X } from 'lucide-react';
import Button from "./ui/Button";
import { useEffect, useRef, useState } from "react";
import { AnimatePresence, motion } from 'framer-motion';
import useClickOutside from "../hooks/useClickOutside";
import useTailwindMediaQuery from "../hooks/useTailwindMediaQuery";

/**
 * The top navigation component
*/
const TopNav = () => {
    const [isOpen, setIsOpen] = useState<boolean>(false);

    const menuRef = useRef<HTMLDivElement | null>(null);
    const menuButtonRef = useRef<HTMLDivElement | null>(null);

    const routes = router.routes;

    useClickOutside([menuRef, menuButtonRef], () => setIsOpen(false));

    useTailwindMediaQuery("md", () => setIsOpen(false));

    const linkClassNames: ((props: NavLinkRenderProps) => string | undefined) | undefined = ({ isActive }) => {
        const baseStyle = "py-2 px-5 flex gap-2 items-center justify-center rounded-lg";

        if (isActive) {
            return `text-primary bg-blue-50 ${baseStyle}`;
        }

        return `text-gray hover:bg-gray-100 hover:text-gray-900 ${baseStyle}`;
    }

    const navLinks = routes[0].children!
        .filter(r => r.path)
        .filter(r => r.handle?.label)
        .filter(r => r.handle?.icon)
        .map(({ path, handle }) => {
            const Icon = handle.icon;
            return (
                <li key={path}><NavLink className={linkClassNames} key={path} to={handle.navPath ?? path!}><Icon />{handle.label}</NavLink></li>
            )
        });

    useEffect(() => {
        setIsOpen(false);
    }, [location.pathname]);

    return (
        <nav className={`fixed top-0 left-0 right-0 z-50 text-lg bg-white`}>
            <div className="w-full h-15 mx-auto px-(--default-margin-x) mt-4 border-b border-gray-200 transition-all duration-500">
                <div className="flex justify-between gap-5">

                    <div className="flex flex-row gap-2 items-center">
                        <Package size="35" className="text-primary" />
                        <div className="flex flex-col">
                            <h1>StockFlow</h1>
                            <p>Lagerstyringssystem</p>
                        </div>
                    </div>

                    <ul className="hidden md:flex gap-2">
                        {navLinks}
                    </ul>

                    <div className="md:hidden" ref={menuButtonRef}>
                        <Button variant="ghost" icon={isOpen ? X : Menu} onClick={() => setIsOpen(prev => !prev)} />
                    </div>
                </div>
            </div>

            <AnimatePresence>
                {isOpen && (
                    <motion.div
                        initial={{ height: 0, opacity: 0.5 }}
                        animate={{ height: "auto", opacity: 1 }}
                        exit={{ height: 0, opacity: 0.5 }}
                        style={{ overflow: "hidden" }}
                        transition={{ duration: 0.2 }}
                        ref={menuRef}
                        className={`md:hidden px-4 text-muted-foreground bg-background/80 mt-3`}
                    >
                        <ul className="border-t border-primary/30 flex flex-col gap-2 px-2 py-4">
                            {navLinks}
                        </ul>
                    </motion.div>
                )}
            </AnimatePresence>
        </nav>
    )
}

export default TopNav;