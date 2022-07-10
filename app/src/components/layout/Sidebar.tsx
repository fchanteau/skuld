import { Link } from "react-router-dom";
import { DropdownMenu, DropdownToggle, Nav, NavItem, NavLink, UncontrolledDropdown } from "reactstrap";

export function Sidebar() {
    const items = itemsMenuBuilder();

    return (
        <Nav vertical className="sidebar shadow position-relative">
            {items.map((item) => <ItemMenu item={item} key={item.label} />)}
        </Nav>
    );
}

interface ItemMenuProps {
    item: IItemMenu;
}

function ItemMenu(props: ItemMenuProps) {
    const { item } = props;

    return item.children ? <DropdownItemMenu item={item}/> : <LinkItemMenu item={item} />
}

interface LinkItemMenuProps extends ItemMenuProps {}

function LinkItemMenu(props: LinkItemMenuProps) {
    const { item } = props;

    return (
        <NavItem>
            <NavLink tag={Link} to={item.to}>{item.label}</NavLink>
        </NavItem>
    )
}

interface DropdownItemMenuProps extends ItemMenuProps {}

function DropdownItemMenu(props: DropdownItemMenuProps) {
    const { item } = props;

    // si on veut le trigger au moment du hover, utiliser Dropdown avec les event associés onMouseEnter, etc...
    // https://github.com/reactstrap/reactstrap/issues/1088
    return (
        <UncontrolledDropdown nav direction="end">
            <DropdownToggle caret nav>
                {item.label}
            </DropdownToggle>
            <DropdownMenu>
                {item.children?.map((child) => <Nav vertical key={child.label}><ItemMenu item={child} /></Nav>)}
            </DropdownMenu>
        </UncontrolledDropdown>
    );
}

interface IItemMenu {
    to?: string;
    label: string;
    children?: IItemMenu[];
}

function itemsMenuBuilder(): IItemMenu[] {
    return [
        {
            to: "/",
            label: "Home"
        },
        {
            to: "/about",
            label: "About"
        },       
        {
            label: "Links",
            children: [
                {
                    label: "Link 1",
                    to: "/",
                    children: [
                        {
                            label: "Link 1.1",
                            to: "/"
                        }
                    ]
                },
                {
                    label: "Link 2",
                    to: "/"
                }
            ]
        }
    ]
}