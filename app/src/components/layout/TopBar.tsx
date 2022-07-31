import { SkuldLogo } from "@/components/shared";
import { UserInfos } from "@/components/features/auth";
import S from "./Layout.module.scss";

export function TopBar() {
    return <div className={`${S.topbar} d-flex justify-content-between align-items-center shadow position-relative`}>
      <SkuldLogo outline width={150} />
      <UserInfos />
    </div>
  }