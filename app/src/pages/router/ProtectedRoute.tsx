import { type PropsWithChildren } from 'react';
import { Navigate } from 'react-router-dom';

import { type Role, useCurrentUserQuery } from '@/features/auth';

export type ProtectedRouteProps = {
  minRole: Role;
  redirect?: string;
};

export function ProtectedRoute(props: PropsWithChildren<ProtectedRouteProps>) {
  const { data: user } = useCurrentUserQuery();

  if (!user || user?.role < props.minRole) {
    return <Navigate to={props.redirect ?? '/'} replace />;
  }

  return props.children;
}
