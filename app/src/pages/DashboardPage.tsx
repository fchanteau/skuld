import { SkuldLogo } from '@/common/components';
import { WidgetUserInfos } from '@/features/user/components/WidgetUserInfos';

export function DashboardPage() {
  return (
    <div className="home d-flex skuld-height justify-content-center align-items-center text-center">
      <SkuldLogo width={500} height={500} />
      <WidgetUserInfos />
    </div>
  );
}
