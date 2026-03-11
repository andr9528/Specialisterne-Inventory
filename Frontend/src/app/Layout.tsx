import TopNav from "../shared/components/TopNav";
import { Outlet, useLocation } from "react-router-dom";

/**
 * Layout component
 */
const Layout = () => {
    const location = useLocation();

    return (
        <div className="bg-background w-full h-full">
            <TopNav />
            <main>
                <Outlet key={location.pathname} />
            </main>
        </div>
    )
}

export default Layout;