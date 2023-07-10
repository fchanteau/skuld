import { type PropsWithChildren } from 'react';
import { Navigate } from 'react-router-dom';

import { type Role, useCurrentUserQuery } from '@/features/auth';

export type ProtectedRouteProps = {
  minRole: Role;
  redirect?: string;
};

export function ProtectedRoute(props: PropsWithChildren<ProtectedRouteProps>) {
  const { data: user, isLoading } = useCurrentUserQuery();

  if (isLoading) return <h1>Chargement...</h1>;

  if (!user || user?.role < props.minRole) {
    return <Navigate to={props.redirect ?? '/'} replace />;
  }

  return props.children;
}
