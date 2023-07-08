import { Outlet } from 'react-router-dom';

import { TopBar } from './TopBar';

export function Layout() {
  return (
    <div>
      <TopBar />
      <div className="flex-grow-1 main">
        <Outlet />
      </div>
    </div>
  );
}
