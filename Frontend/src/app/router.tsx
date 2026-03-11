import { createBrowserRouter, Navigate } from "react-router-dom";
import { LayoutDashboard, Package } from 'lucide-react';
import Layout from "./Layout";

export const router = createBrowserRouter([
    {
        element: <Layout />,
        children: [
            { path: "/", element: <></>, handle: { label: "Dashboard", icon: LayoutDashboard } },
            { path: "/products", element: <></>, handle: { label: "Produkter", icon: Package } },
            { path: "*", element: <Navigate to="/" replace /> },
        ]
    }
])