import { useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";
import { Button, CloseButton, Form, FormFeedback, FormGroup, Input, Label, Offcanvas, OffcanvasBody, OffcanvasHeader, Spinner } from "reactstrap";
import { actionCreators } from "../store/actions";
import { displayLogin } from "../store/display/displaySelectors";
import { SkuldLogo } from "./shared/SkuldLogo";

export function Login() {
    const dispatch = useDispatch();
    const login = useSelector(displayLogin);

    const onClickCloseButton = () => {
        dispatch(actionCreators.display.toggleLogin());
    }

    return (
        <Offcanvas
            direction="end" isOpen={login.show}>
            <OffcanvasHeader>
                <SkuldLogo width="90%" />
                <CloseButton onClick={onClickCloseButton} />
            </OffcanvasHeader>
            <OffcanvasBody>
                <LoginForm loading={login.loading}  />
            </OffcanvasBody>
        </Offcanvas>
    );
}

interface LoginFormInput {
    email: string;
    password: string;
}

interface LoginFormProps {
    loading: boolean;
}

function LoginForm(props: LoginFormProps) {
    const { loading } = props;
    const dispatch = useDispatch();
    const { control, handleSubmit, formState: { errors } } = useForm<LoginFormInput>();

    const onSubmit: SubmitHandler<LoginFormInput> = data => {
        dispatch(actionCreators.display.toggleLoginLoading());
        dispatch(actionCreators.users.login(data));
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
                {errors.email?.type === 'required' && "fzdsvffsde" }
                {errors.email?.type === 'required' &&
                    <FormFeedback>
                        Oh noes! that name is already taken
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
            </FormGroup>
            <Button color="primary" className="w-100">
                {
                    loading ?
                        <Spinner color="dark" /> :
                        "Submit"
                }
            </Button>
            <pre>{JSON.stringify(errors)}</pre>
        </Form>
    )
}