import { SkuldLogo } from "@/components/shared";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { actionCreators } from "@/store";
import { getDisplayAuth } from "@/store/display";
import { Button, Offcanvas, OffcanvasBody, OffcanvasHeader } from "reactstrap";
import { Login } from "./Login";
import { Register } from "./Register";

export function Auth() {
    const dispatch = useAppDispatch();
    const displayAuth = useAppSelector(getDisplayAuth);
    const form = displayAuth.isLogin ? <Login /> : <Register />;

    const switchForm = () => {
        dispatch(actionCreators.display.toggleAuthLogin());
    }

    return (
        <Offcanvas
            direction="end" isOpen={displayAuth.show}>
            <OffcanvasHeader>
                <SkuldLogo width="90%" />
            </OffcanvasHeader>
            <OffcanvasBody>
                {form}
                <hr />
                <Button className="w-100" color="primary" outline onClick={switchForm}>Create an account</Button>
            </OffcanvasBody>
        </Offcanvas>
    )
}