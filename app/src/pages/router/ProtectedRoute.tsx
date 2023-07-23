import { type PropsWithChildren } from 'react';
import { Navigate } from 'react-router-dom';

import { PongLoading } from '@/common/components/PongLoading';
import { type Role, useCurrentUserQuery } from '@/features/user';

export type ProtectedRouteProps = {
  minRole: Role;
  redirect?: string;
};

export function ProtectedRoute(props: PropsWithChildren<ProtectedRouteProps>) {
  const { data: user, isLoading, isUninitialized } = useCurrentUserQuery();

  // TODO : Show an overlay component for loading, componet should be positoin absolute
  if (isLoading || isUninitialized) return <PongLoading />;

  if (!user || user?.role < props.minRole) {
    return <Navigate to={props.redirect ?? '/'} replace />;
  }

  return props.children;
}
