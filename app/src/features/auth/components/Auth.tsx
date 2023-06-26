import { Button, Offcanvas, OffcanvasBody, OffcanvasHeader } from "reactstrap";
import { Login } from "./Login";
import { Register } from "./Register";
import { SkuldLogo } from "@/common/components";
import { AuthType } from "@/pages/LandingPage";
import { useSelector } from "react-redux";
import { isConnected } from "../auth.selector";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

type AuthProps = {
    type: AuthType;
    onChangeType: (type: AuthType) => void;
};

export function Auth(props: AuthProps) {
    const navigate = useNavigate();
    const userIsConnected = useSelector(isConnected);

    useEffect(() => {
        if (userIsConnected)
        navigate('/');

    }, [userIsConnected]);

    const isSignIn: boolean = props.type === 'SignIn';

    const form = isSignIn ? <Login /> : <Register />;

    const switchForm = () => {
        const newType: AuthType = isSignIn ? 'SignUp' : 'SignIn';
        props.onChangeType(newType);
    }

    return (
        <Offcanvas
            direction="end"
            isOpen>
            <OffcanvasHeader>
                <SkuldLogo width="90%" />
            </OffcanvasHeader>
            <OffcanvasBody>
                {form}
                <hr />
                <Button className="w-100" color="primary" outline onClick={switchForm}>{ isSignIn ? "Register now !" : "Already have an account ? Sign in !" }</Button>
            </OffcanvasBody>
        </Offcanvas>
    )
}