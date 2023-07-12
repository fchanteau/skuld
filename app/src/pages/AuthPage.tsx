import { useState } from 'react';

import bkg from '@/assets/bg-auth.jpg';
import { Auth } from '@/features/auth';

export type AuthType = 'SignIn' | 'SignUp';

export function AuthPage() {
  const [authType, setAuthType] = useState('SignIn' as AuthType);

  return (
    <div className="container-fluid g-0 auth">
      <div className="row g-0 align-items-stretch min-vh-100">
        <div className="col">
          <div className="row g-0 align-items-start h-100">
            <div className="col mx-5">
              <Auth type={authType} onChangeType={setAuthType} />
            </div>
          </div>
        </div>
        <div
          className="col presentation text-white position-relative d-none d-xl-block"
          style={{ background: `url(${bkg}) no-repeat center center`, backgroundSize: 'cover' }}
        >
          <div className="overlay position-absolute top-0 start-0 w-100 h-100"></div>
          <div className="row g-0 align-items-center w-100 h-100">
            <div className="col text-center presentation-container">
              <h2>Welcome</h2>
              <hr className="w-25 mx-auto" />
              <p>
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Tenetur libero vero odit autem nam, mollitia
                ducimus facilis fugit molestiae consequatur expedita quisquam veritatis dolor facere ab! Incidunt
                tenetur nemo eligendi?
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
