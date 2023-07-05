import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { SkuldLogo } from '@/common/components';
import { isConnected } from '@/features/auth/auth.selector';

export function DashboardPage() {
  const navigate = useNavigate();

  const userIsConnected = useSelector(isConnected);

  useEffect(() => {
    if (!userIsConnected) {
      console.log('FCU go to landing');
      navigate('/landing');
    }
  }, [navigate, userIsConnected]);

  return (
    <div className="home d-flex skuld-height justify-content-center align-items-center text-center">
      <SkuldLogo width={500} height={500} />
    </div>
  );
}
