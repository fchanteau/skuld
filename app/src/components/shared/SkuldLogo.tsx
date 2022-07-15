import { ReactComponent as Logo } from '../../svgs/Skuld.svg';
import { ReactComponent as WhiteLogo } from '../../svgs/Skuld-white.svg';
import { useNavigate } from 'react-router-dom';

interface SkuldLogoProps {
    outline?: boolean;
    width?: number;
    height?: number;
}

const defaultProps: SkuldLogoProps = {
    outline: false,
    width: 2,
    height: 1
}

export function SkuldLogo(props: SkuldLogoProps = defaultProps) {
    const { width, height, outline } = props;
    const navigate = useNavigate();
    
    const onClick = () => {
        navigate('/');
    }

    return outline ? <WhiteLogo width={width} height={height} onClick={onClick} /> : <Logo width={width} height={height} onClick={onClick} />
}