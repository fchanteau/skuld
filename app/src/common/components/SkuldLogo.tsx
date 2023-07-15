import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { isConnected } from '@/features/auth/auth.selector';
import { ReactComponent as Logo } from '@/svgs/tth.svg';

interface SkuldLogoProps {
  width?: string | number;
  height?: number;
}

const defaultProps: SkuldLogoProps = {
  width: 200,
  height: 200,
};

export function SkuldLogo(props: SkuldLogoProps = defaultProps) {
  const { width, height } = props;
  const navigate = useNavigate();
  const userIsConnected = useSelector(isConnected);

  const onClick = () => {
    userIsConnected ? navigate('/') : navigate('/auth');
  };

  return <Logo width={width} height={height} onClick={onClick} />;
}
