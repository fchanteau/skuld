import { Link } from "react-router-dom";
import { Nav, NavItem, NavLink } from "reactstrap";

export function Sidebar() {
    const items = itemsMenu();

    return (
        <Nav vertical className="sidebar">
            {items.map((item, index) => {
                return (
                    <NavItem key={index}>
                        <NavLink tag={Link} to={item.to}>{item.label}</NavLink>
                    </NavItem>
                );
            })}
        </Nav>
    );
}

function itemsMenu() {
    return [
        {
            to: "/",
            label: "Home"
        },
        {
            to: "/about",
            label: "About"
        }
    ]
}