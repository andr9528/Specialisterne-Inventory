import TopNav from "../shared/components/TopNav";
import { Outlet, useLocation } from "react-router-dom";

/**
 * Layout component
 */
const Layout = () => {
    const location = useLocation();

    return (
        <div className="bg-background w-screen h-screen flex flex-col overflow-hidden">
            <TopNav />
            <main className="flex-1 overflow-y-auto">
                <Outlet key={location.pathname} />
            </main>
        </div>
    )
}

export default Layout;