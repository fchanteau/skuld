import { useCurrentUserQuery } from '../auth.api';

export function UserInfos() {
  const { data } = useCurrentUserQuery();

  return (
    <div className="me-3 text-primary">
      <h5>
        {data?.firstName} {data?.lastName}
      </h5>
    </div>
  );
}
