import { UserInfos } from "../UserInfos";

export function TopBar() {
    return <div className='topbar d-flex justify-content-between align-items-center shadow position-relative'>
      <h1 className='ms-1 my-1'>Skuld</h1>
      <UserInfos />
    </div>
  }