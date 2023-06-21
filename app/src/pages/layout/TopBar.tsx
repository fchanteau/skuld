import { SkuldLogo } from "@/common/components";
import { UserInfos } from "@/features/auth";

export function TopBar() {
    return <div className='topbar d-flex justify-content-between align-items-center shadow position-relative'>
      <SkuldLogo outline width={150} />
      <UserInfos />
    </div>
  }