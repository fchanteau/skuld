import { Outlet } from "react-router-dom";

import { Sidebar } from "./Sidebar";
import { TopBar } from "./TopBar";

export function Layout() {
  return (
    <>
      <TopBar />
      <div className='d-flex align-items-stretch'>
        <Sidebar />
        <div className="flex-grow-1 main">
          <Outlet />
        </div>
      </div>
    </>
  );
}