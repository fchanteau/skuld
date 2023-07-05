import { UserInfos } from '@/features/auth';

export function TopBar() {
  return (
    <div className="topbar d-flex justify-content-end align-items-center position-relative">
      <UserInfos />
    </div>
  );
}
