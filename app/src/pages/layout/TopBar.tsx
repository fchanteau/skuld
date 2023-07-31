import { useState } from 'react';
import { FormattedMessage } from 'react-intl';
import { Link, useNavigate } from 'react-router-dom';
import {
  Collapse,
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
  Nav,
  Navbar,
  NavbarBrand,
  NavbarText,
  NavbarToggler,
  NavItem,
  NavLink,
  UncontrolledDropdown,
} from 'reactstrap';

import { actionCreators } from '@/bootstrap';
import { useFormatMessage } from '@/common/hooks';
import { clearStorage } from '@/features/auth';
import { useCurrentUserQuery } from '@/features/user';
import { useAppDispatch } from '@/hooks';

export function TopBar() {
  const items = useItemsMenuBuilder();

  const [collapsed, setCollapsed] = useState(true);

  const toggleNavbar = () => setCollapsed(!collapsed);

  return (
    <div>
      <Navbar color="primary" expand="xl" dark container="fluid">
        {/* navbar navbar-expand-xl navbar-light navbar-dark bg-primary */}
        <NavbarBrand>
          <FormattedMessage id="common.title" />
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} />
        <Collapse isOpen={!collapsed} navbar>
          <Nav className="me-auto" navbar>
            {items.map((item) => (
              <ItemMenu item={item} key={item.label} />
            ))}
          </Nav>
          <NavbarText>
            <UserMenu />
          </NavbarText>
        </Collapse>
      </Navbar>
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
        <i className={`pe-2 bi bi-${item.icon}`}></i>
        <span>{item.label}</span>
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
          <DropdownItem key={child.label}>
            <ItemMenu item={child} />
          </DropdownItem>
        ))}
      </DropdownMenu>
    </UncontrolledDropdown>
  );
}

function UserMenu() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { data: user } = useCurrentUserQuery();

  const onLogout = () => {
    clearStorage();
    dispatch(actionCreators.auth.setConnectedUser(false));
    navigate('/auth');
  };

  return (
    <UncontrolledDropdown>
      <DropdownToggle nav>
        <i className="pe-2 bi bi-person-circle"></i> {user?.lastName} {user?.firstName}
      </DropdownToggle>
      <DropdownMenu>
        <DropdownItem onClick={onLogout}>
          <FormattedMessage id="auth.logout" />
        </DropdownItem>
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

const useItemsMenuBuilder = (): IItemMenu[] => {
  const formatMessage = useFormatMessage();

  return [
    {
      to: '/',
      label: formatMessage('topbar.dashboard'),
      icon: 'speedometer',
    },
    {
      to: '/auth',
      label: 'Auth',
      icon: 'house-door-fill',
    },
    {
      to: '/protected',
      label: 'Protected',
      icon: 'house-door-fill',
    },
  ];
};
