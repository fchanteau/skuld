import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from "reactstrap";
import { useLoginMutation, UserLoginPayload } from "@/api/users";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { actionCreators } from "@/store";
import { Error } from "@/components/shared";

interface LoginFormInput {
    email: string;
    password: string;
}

export function LoginForm() {
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