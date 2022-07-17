import { useCurrentUserQuery } from "@/api/users";

export function UserInfos() {
    const { data } = useCurrentUserQuery();

    return (
        <div className="me-3">
            <h5>{data?.firstName} {data?.lastName}</h5>
        </div>
    )
  }