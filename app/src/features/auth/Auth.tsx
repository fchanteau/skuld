import { SkuldLogo } from "@/components/shared";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { actionCreators } from "@/store";
import { getDisplayAuth } from "@/store/display";
import { Button, Offcanvas, OffcanvasBody, OffcanvasHeader } from "reactstrap";
import { Login } from "./Login";
import { Register } from "./Register";

export function Auth() {
    const dispatch = useAppDispatch();
    const { isSignIn, show } = useAppSelector(getDisplayAuth);
    const form = isSignIn ? <Login /> : <Register />;

    const switchForm = () => {
        if (isSignIn) {
            dispatch(actionCreators.display.showSignUp());
        }
        else {
            dispatch(actionCreators.display.showSignIn());
        }
    }

    return (
        <Offcanvas
            direction="end" isOpen={show}>
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