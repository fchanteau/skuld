import { useMemo } from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { Alert, Button } from 'reactstrap';

import { SkuldLogo } from '@/common/components';
import { useFormatMessage } from '@/common/hooks';
import { type AuthType } from '@/pages/AuthPage';

import { isConnected } from '../auth.selector';

import { Login } from './Login';
import { Register } from './Register';

type AuthProps = {
  type: AuthType;
  onChangeType: (type: AuthType) => void;
};

export function Auth(props: AuthProps) {
  const userIsConnected = useSelector(isConnected);
  const formatMessage = useFormatMessage();

  const isSignIn: boolean = useMemo(() => props.type === 'SignIn', [props.type]);

  const form = isSignIn ? <Login /> : <Register />;

  const switchForm = () => {
    const newType: AuthType = isSignIn ? 'SignUp' : 'SignIn';
    props.onChangeType(newType);
  };

  if (userIsConnected) {
    return (
      <Alert className="d-flex justify-content-center mt-5" color="info">
        Already logged. <Link to="/">Go to home page</Link>
      </Alert>
    );
  }

  return (
    <>
      <div className="d-flex justify-content-center">
        <SkuldLogo width={300} />
      </div>
      {form}
      <hr />
      <Button block color="primary" outline onClick={switchForm}>
        {isSignIn ? formatMessage('auth.register.phrase') : formatMessage('auth.login.phrase')}
      </Button>
    </>
  );
}
