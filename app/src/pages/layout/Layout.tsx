import { TopBar } from "./TopBar";
import { Sidebar } from "./Sidebar";
import { Outlet } from "react-router-dom";

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