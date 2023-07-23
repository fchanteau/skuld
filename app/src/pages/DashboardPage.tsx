import { WidgetUserInfos } from '@/features/user/components/WidgetUserInfos';

export function DashboardPage() {
  return (
    <div className="home container-fluid mt-5">
      <div className="row">
        <div className="col-12 col-xl-6">
          <WidgetUserInfos />
        </div>
      </div>
    </div>
  );
}
