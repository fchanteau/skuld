import { useSelector } from "react-redux";
import { Navbar, NavbarBrand, NavbarText } from "reactstrap";
import { getUserInfos } from "../../store/users/usersSelectors";

export function Header() {
    const userInfos = useSelector(getUserInfos);
    return (
        <Navbar
            color="primary"
            light>
            <NavbarBrand>
                Skuld
            </NavbarBrand>
            <NavbarText>
                Welcome {userInfos?.firstName} {userInfos?.lastName} !
            </NavbarText>
        </Navbar>
        )
    }