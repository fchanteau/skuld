import { createBrowserRouter } from 'react-router-dom';

import { Role } from '@/features/user';

import { AuthPage } from '../AuthPage';
import ChakraPage from '../ChakraPage';
import { DashboardPage } from '../DashboardPage';
import { Layout } from '../layout';

import { ProtectedRoute } from './ProtectedRoute';

export const router = createBrowserRouter([
  {
    id: 'auth',
    path: '/auth',
    element: <AuthPage />,
  },
  {
    id: 'chakra',
    path: '/chakra',
    element: <ChakraPage />,
  },
  {
    id: 'root',
    path: '/',
    element: <Layout />,
    children: [
      {
        index: true,
        element: (
          <ProtectedRoute minRole={Role.User} redirect="/auth">
            <DashboardPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'protected',
        element: (
          <ProtectedRoute minRole={Role.Admin}>
            <h1>Protected</h1>
          </ProtectedRoute>
        ),
      },
    ],
  },
]);
