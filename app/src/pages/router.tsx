import { createBrowserRouter } from 'react-router-dom';

import { AuthPage } from './AuthPage';
import { DashboardPage } from './DashboardPage';
import { Layout } from './layout';

export const router = createBrowserRouter([
  {
    id: 'auth',
    path: '/auth',
    element: <AuthPage />,
  },
  {
    id: 'root',
    path: '/',
    element: <Layout />,
    children: [
      {
        index: true,
        element: <DashboardPage />,
      },
    ],
  },
]);
