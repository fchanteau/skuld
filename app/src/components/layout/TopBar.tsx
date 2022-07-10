import { SkuldLogo } from "../shared/SkuldLogo";
import { UserInfos } from "../UserInfos";

export function TopBar() {
    return <div className='topbar d-flex justify-content-between align-items-center shadow position-relative'>
      <SkuldLogo outline width={150} />
      <UserInfos />
    </div>
  }