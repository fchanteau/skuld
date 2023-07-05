import { Outlet } from 'react-router-dom';

import { Sidebar } from './Sidebar';

export function Layout() {
  return (
    <div className="d-flex align-items-stretch">
      <Sidebar />
      <div className="flex-grow-1 main">
        <Outlet />
      </div>
    </div>
  );
}
