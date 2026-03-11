import { createBrowserRouter, Navigate } from "react-router-dom";
import { LayoutDashboard, Package } from 'lucide-react';
import Layout from "./Layout";
import DashboardPage from "../features/dashboard/pages/DashboardPage";
import ProductsPage from "../features/products/pages/ProductsPage";

export const router = createBrowserRouter([
    {
        element: <Layout />,
        children: [
            { path: "/", element: <DashboardPage />, handle: { label: "Dashboard", icon: LayoutDashboard } },
            { path: "/products", element: <ProductsPage />, handle: { label: "Produkter", icon: Package } },
            { path: "*", element: <Navigate to="/" replace /> },
        ]
    }
])