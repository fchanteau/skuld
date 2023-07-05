import { useEffect, useMemo } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { Button, Offcanvas, OffcanvasBody, OffcanvasHeader } from 'reactstrap';

import { SkuldLogo } from '@/common/components';
import { type AuthType } from '@/pages/LandingPage';

import { isConnected } from '../auth.selector';

import { Login } from './Login';
import { Register } from './Register';

type AuthProps = {
  type: AuthType;
  onChangeType: (type: AuthType) => void;
};

export function Auth(props: AuthProps) {
  const navigate = useNavigate();
  const userIsConnected = useSelector(isConnected);

  useEffect(() => {
    if (userIsConnected) navigate('/');
  }, [navigate, userIsConnected]);

  const isSignIn: boolean = useMemo(() => props.type === 'SignIn', [props.type]);

  const form = isSignIn ? <Login /> : <Register />;

  const switchForm = () => {
    const newType: AuthType = isSignIn ? 'SignUp' : 'SignIn';
    props.onChangeType(newType);
  };

  return (
    <Offcanvas direction="end" isOpen fade>
      <OffcanvasHeader>
        <SkuldLogo width="100%" />
      </OffcanvasHeader>
      <OffcanvasBody>
        {form}
        <hr />
        <Button block color="primary" outline onClick={switchForm}>
          {isSignIn ? 'Register now !' : 'Already have an account ? Sign in !'}
        </Button>
      </OffcanvasBody>
    </Offcanvas>
  );
}
