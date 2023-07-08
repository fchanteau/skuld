import { useState } from 'react';

import bkg from '@/assets/bg-auth.jpg';
import { Auth } from '@/features/auth';

export type AuthType = 'SignIn' | 'SignUp';

export function LandingPage() {
  const [authType, setAuthType] = useState('SignIn' as AuthType);

  // return (
  //   <div
  //     className="landing min-vh-100 d-flex flex-column justify-content-center align-items-center text-center"
  //     style={{ backgroundImage: `url(${bkg})` }}
  //   >
  //     <div className="overlay position-absolute top-0 start-0 w-100 h-100"></div>
  //     <div className="landing-logo">
  //       <SkuldLogo width="400" />
  //     </div>
  //     <div className="landing-container">
  //       <p className="display-6">The best companion for small and medium businesses</p>
  //       <div className="action-buttons">
  //         <Button color="primary" className="mx-1" onClick={() => setAuthType('SignIn')}>
  //           Sign In
  //         </Button>
  //         <Button color="primary" className="mx-1" outline onClick={() => setAuthType('SignUp')}>
  //           Sign Up
  //         </Button>
  //       </div>
  //     </div>
  //     {authType && <Auth type={authType} onChangeType={setAuthType} />}
  //   </div>
  // );

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
          className="col presentation text-white position-relative"
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
