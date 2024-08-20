import { useNavigate } from 'react-router-dom';

import Tth from '@/svgs/tth.svg';

interface SkuldLogoProps {
  width?: string | number;
  height?: number;
}

const defaultProps: SkuldLogoProps = {
  width: 200,
  height: 200,
};

export function SkuldLogo(props: SkuldLogoProps = defaultProps): JSX.Element {
  // const { width, height } = props;
  // const navigate = useNavigate();

  // const onClick = () => {
  //   navigate('/');
  // };

  return <Tth />;
}
