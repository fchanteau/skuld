import { useState } from 'react';
import { Button } from 'reactstrap';

import bkg from '@/assets/bg-landing.png';
import { SkuldLogo } from '@/common/components';
import { Auth } from '@/features/auth';

export type AuthType = 'SignIn' | 'SignUp' | undefined;

export function LandingPage() {
  const [authType, setAuthType] = useState(undefined as AuthType);

  return (
    <div
      className="landing min-vh-100 d-flex justify-content-center align-items-center text-center"
      style={{ backgroundImage: `url(${bkg})` }}
    >
      <div className="overlay position-absolute top-0 start-0 w-100 h-100"></div>
      <div className="fixed-top">
        <SkuldLogo width="500" />
      </div>
      <div className="landing-container">
        <p className="display-6">The best companion for small and medium businesses</p>
        <div className="action-buttons">
          <Button color="primary" className="mx-1" onClick={() => setAuthType('SignIn')}>
            Sign In
          </Button>
          <Button color="primary" className="mx-1" outline onClick={() => setAuthType('SignUp')}>
            Sign Up
          </Button>
        </div>
      </div>
      {authType && <Auth type={authType} onChangeType={setAuthType} />}
    </div>
  );
}
