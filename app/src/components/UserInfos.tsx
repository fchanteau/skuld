import { useCurrentUserQuery } from "../api/users/users";

export function UserInfos() {
    const { data } = useCurrentUserQuery();

    return (
        <div className="me-3">
            <h5>{data?.firstName} {data?.lastName}</h5>
        </div>
    )
  }