import { NavLink, type NavLinkRenderProps } from "react-router-dom";
import { router } from "../../app/router";

/**
 * The top navigation component
*/
const TopNav = () => {

    const routes = router.routes;

    const linkClassNames: ((props: NavLinkRenderProps) => string | undefined) | undefined = ({ isActive }) => {
        const baseStyle = "py-2 px-5 flex gap-2 items-center justify-center rounded-lg";
        
        if (isActive) {
            return `text-primary bg-blue-50 ${baseStyle}`;
        }
        
        return `text-gray-600 hover:bg-gray-100 hover:text-gray-900 ${baseStyle}`;
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

    return (
        <nav className={`fixed top-0 left-0 right-0 z-50 text-lg bg-white`}>
            <div className="w-full mx-auto px-4 mt-5">
                <div className="flex justify-end gap-5">
                    <ul className="flex gap-2">
                        {navLinks}
                    </ul>
                </div>
            </div>
        </nav>
    )
}

export default TopNav;