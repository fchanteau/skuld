import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { Alert, Button, Form, FormFeedback, FormGroup, Input, Label, Offcanvas, OffcanvasBody, OffcanvasHeader, Spinner } from "reactstrap";
import { useCurrentUserQuery, useLoginMutation, UserLoginPayload } from "../api/users/users";
import { useAppDispatch, useAppSelector } from "../hooks/store";
import { actionCreators } from "../store/actions";
import { displayLogin } from "../store/display/displaySelectors";
import { SkuldLogo } from "./shared/SkuldLogo";
import { Error } from "./shared/Error";

export function Login() {
    //const dispatch = useAppDispatch();
    const login = useAppSelector(displayLogin);

    // const onClickCloseButton = () => {
    //     dispatch(actionCreators.display.toggleLogin());
    // }

    return (
        <Offcanvas
            direction="end" isOpen={login.show}>
            <OffcanvasHeader>
                <SkuldLogo width="90%" />
                {/* <CloseButton onClick={onClickCloseButton} /> */}
            </OffcanvasHeader>
            <OffcanvasBody>
                <LoginForm />
            </OffcanvasBody>
        </Offcanvas>
    );
}

interface LoginFormInput {
    email: string;
    password: string;
}


function LoginForm() {
    const dispatch = useAppDispatch();
    const [login, { isLoading, error }] = useLoginMutation();   
    const { control, handleSubmit, formState: { errors } } = useForm<LoginFormInput>();

    const onSubmit: SubmitHandler<LoginFormInput> = async data => {
        const payload: UserLoginPayload = {
            email: data.email,
            password: data.password
        };
        const tokenInfos = await login(payload).unwrap();
        dispatch(actionCreators.users.setTokenInfos(tokenInfos));
      };

    return (
        <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup floating>
                <Controller
                    name="email"
                    control={control}
                    rules={{ required: true, minLength: 8 }}
                    render={({field}) => <Input invalid={errors.email?.type === 'required'} id="email" type="email" {...field} />}
                />
                <Label for="email">
                    Email*
                </Label>
                {errors.email?.type === 'required' &&
                    <FormFeedback>
                        Email is required
                    </FormFeedback>
                }
            </FormGroup>
            <FormGroup floating>
                <Controller
                    name="password"
                    control={control}
                    rules={{ required: true }}
                    render={({field}) => <Input invalid={errors.password?.type === 'required'} id="password" type="password" {...field} />}
                />
                <Label for="password">
                    Password*
                </Label>
                {errors.password?.type === 'required' &&
                    <FormFeedback>
                        Password is required
                    </FormFeedback>
                }
            </FormGroup>
            <Button color="primary" className="w-100 mb-3">
                {
                    isLoading ?
                        <Spinner color="dark" /> :
                        "Submit"
                }
            </Button>
            
            {error && 
                <Error error={error} />
            }
        </Form>
    )
}