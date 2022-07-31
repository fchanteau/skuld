import { useDispatch } from 'react-redux';
import { Button } from 'reactstrap';


import { actionCreators } from '@/store';
import { Auth } from '@/components/features/auth';
import { SkuldLogo } from '@/components/shared';
import S from "./Layout.module.scss";
import bkg from '@/assets/bg-landing.png';

export function LandingPage() {
    const dispatch = useDispatch();
    const onClickSignin = () => {
        dispatch(actionCreators.display.showSignIn());
    }
    const onClickSignUp = () => {
        dispatch(actionCreators.display.showSignUp());
    }

    return (
      <div className={`${S.landing} min-vh-100 d-flex justify-content-center align-items-center text-center`} style={{backgroundImage: `url(${bkg})`}}>
        <div className={`${S.overlay} position-absolute top-0 start-0 w-100 h-100`}></div>
        <div className="fixed-top">
            <SkuldLogo width="500" />
        </div>
        <div className={`${S.landingContainer}`}>
            <p className='display-6'>The best companion for small and medium businesses</p>
            <div className="action-buttons">
                <Button color="primary" className='mx-1' onClick={onClickSignin}>
                    Sign In
                </Button>
                <Button color="primary" className='mx-1' outline onClick={onClickSignUp}>
                    Sign Up
                </Button>
            </div>
        </div>
        <Auth />
      </div>
    )
  }