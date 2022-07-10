import { useSelector } from "react-redux";
import { getUserInfos } from "../store/users/usersSelectors";

export function UserInfos() {
    const userInfos = useSelector(getUserInfos);
    return (
        <h5>{userInfos?.firstName} {userInfos?.lastName}</h5>
    )
  }