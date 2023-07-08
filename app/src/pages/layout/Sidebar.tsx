import { Link } from 'react-router-dom';
import { DropdownMenu, DropdownToggle, Nav, NavItem, NavLink, UncontrolledDropdown } from 'reactstrap';

export function Sidebar() {
  const items = itemsMenuBuilder();

  return (
    <div className="min-vh-100 sidebar position-relative d-flex flex-column border-0 border-end">
      <Nav vertical>
        {items.map((item) => (
          <ItemMenu item={item} key={item.label} />
        ))}
      </Nav>
    </div>
  );
}

interface ItemMenuProps {
  item: IItemMenu;
}

function ItemMenu(props: ItemMenuProps) {
  const { item } = props;

  return item.children ? <DropdownItemMenu item={item} /> : <LinkItemMenu item={item} />;
}

type LinkItemMenuProps = ItemMenuProps;

function LinkItemMenu(props: LinkItemMenuProps) {
  const { item } = props;

  return (
    <NavItem>
      <NavLink tag={Link} to={item.to} className="w-100">
        <i className={`fs-5 pe-2 bi bi-${item.icon}`}></i>
        <span className="fs-5">{item.label}</span>
      </NavLink>
    </NavItem>
  );
}

type DropdownItemMenuProps = ItemMenuProps;

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
        {item.children?.map((child) => (
          <Nav vertical key={child.label}>
            <ItemMenu item={child} />
          </Nav>
        ))}
      </DropdownMenu>
    </UncontrolledDropdown>
  );
}

interface IItemMenu {
  to?: string;
  label: string;
  children?: IItemMenu[];
  icon?: string;
}

function itemsMenuBuilder(): IItemMenu[] {
  return [
    {
      to: '/',
      label: 'Dashboard',
      icon: 'speedometer',
    },
    {
      to: '/auth',
      label: 'Landing',
      icon: 'house-door-fill',
    },
    {
      label: 'Links',
      children: [
        {
          label: 'Link 1',
          to: '/',
          children: [
            {
              label: 'Link 1.1',
              to: '/',
            },
          ],
        },
        {
          label: 'Link 2',
          to: '/',
        },
      ],
    },
  ];
}
