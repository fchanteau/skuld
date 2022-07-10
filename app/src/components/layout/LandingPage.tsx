import { useDispatch } from 'react-redux';
import { Button } from 'reactstrap';


import { actionCreators } from '../../store/actions';
import { SkuldLogo } from '../shared/SkuldLogo';

export function LandingPage() {
    const dispatch = useDispatch();
    const onClickSignin = () => {
        dispatch(actionCreators.users.login({email: "francois.chanteau49@gmail.com", password: "Testtest01"}));
    }

    return (
      <div className="landing min-vh-100 d-flex justify-content-center align-items-center text-center">
        <div className='overlay position-absolute top-0 start-0 w-100 h-100'></div>
        <div className="fixed-top">
            <SkuldLogo width={500} />
        </div>
        <div className='landing-container'>
            <p className='display-6'>The best companion for small and medium businesses</p>
            <div className="action-buttons">
                <Button color="primary mx-1" onClick={onClickSignin}>
                    Sign In
                </Button>
                <Button color="primary mx-1" outline>
                    Sign Up
                </Button>
            </div>
        </div>
      </div>
    )
  }