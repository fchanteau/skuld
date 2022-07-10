import { ReactComponent as Logo } from '../../svgs/Skuld.svg';
import { ReactComponent as WhiteLogo } from '../../svgs/Skuld-white.svg';
import { useNavigate } from 'react-router-dom';

interface SkuldLogoProps {
    outline?: boolean;
    width?: number;
    height?: number;
}

export function SkuldLogo(props: SkuldLogoProps = {outline: false}) {
    const { width, height, outline } = props;
    const navigate = useNavigate();
    
    const onClick = () => {
        navigate('/');
    }

    return outline ? <WhiteLogo width={width} height={height} onClick={onClick} /> : <Logo width={width} height={height} onClick={onClick} />
}