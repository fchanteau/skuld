import { useSelector } from "react-redux";
import { Card, CardImg, CardImgOverlay, CardSubtitle, CardTitle } from "reactstrap";
import { getUserInfos } from "../../store/users/usersSelectors";

export function UserInfos() {
    const userInfos = useSelector(getUserInfos);
    return (
        <Card inverse className="border-0">
            <CardImg
            alt="Card image cap"
            src="https://picsum.photos/320/80"
            width="100%"
            />
            <CardImgOverlay>
                <CardTitle tag="h5">
                    {userInfos?.firstName} {userInfos?.lastName}
                </CardTitle>
                <CardSubtitle>
                    {userInfos?.email}
                </CardSubtitle>
            </CardImgOverlay>
        </Card>
    )
  }