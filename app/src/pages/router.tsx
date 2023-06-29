import { createBrowserRouter } from "react-router-dom";

import { Home } from "./Home";
import { LandingPage } from "./LandingPage";
import { Layout } from "./layout";

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