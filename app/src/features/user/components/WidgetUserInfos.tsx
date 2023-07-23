import { useCurrentUserQuery } from '../user.api';

export function WidgetUserInfos() {
  const { data } = useCurrentUserQuery();

  return (
    <div className="me-3 text-primary">
      <h5>
        {data?.firstName} {data?.lastName}
      </h5>
    </div>
  );
}
