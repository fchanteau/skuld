import { createBrowserRouter } from "react-router-dom";
import { Home } from "./Home";
import { Layout } from "./layout";
import { LandingPage } from "./LandingPage";

export const router = createBrowserRouter([
    {
        id: 'landing',
        path: '/landing',
        element: <LandingPage />
    },
    {
        id: 'root',
        path: '/',
        element: <Layout />,
        children: [
            {
                index: true,
                element: <Home />,
            },
        ],
    }
]);