import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from "reactstrap";
import { useAppDispatch, useAppSelector } from "@/hooks";
import { useNavigate } from "react-router-dom";
import { useLoginMutation } from "../auth.api";
import { UserLoginPayload } from "../auth.model";
import { ErrorMessage } from "@/common/components";
// import * as yup from "yup";
// import { yupResolver } from '@hookform/resolvers/yup';

interface LoginInput {
    email: string;
    password: string;
}

// const schema: yup.AnyObjectSchema = yup.object({
//     email: yup.string().required("Email is required"),
//     password: yup.string().required("Password is required"),
// }).required();

export function Login() {
    const dispatch = useAppDispatch();
    const [login, { isLoading, error }] = useLoginMutation();
    const navigate = useNavigate(); 
    const { control, handleSubmit, formState: { errors } } = useForm<LoginInput>({
        //resolver: yupResolver(schema)
    });

    const onSubmit: SubmitHandler<LoginInput> = async data => {
        const payload: UserLoginPayload = {
            email: data.email,
            password: data.password
        };
        const tokenInfos = await login(payload).unwrap();
        //dispatch(actionCreators.users.setTokenInfos(tokenInfos));
        navigate('/');
      };

    return (
        <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup floating>
                <Controller
                    name="email"
                    control={control}
                    render={({field}) => <Input invalid={errors.email?.type === 'required'} id="email" type="email" {...field} />}
                />
                <Label for="email">
                    Email*
                </Label>
                {errors.email &&
                    <FormFeedback>
                        {errors.email.message}
                    </FormFeedback>
                }
            </FormGroup>
            <FormGroup floating>
                <Controller
                    name="password"
                    control={control}
                    render={({field}) => <Input invalid={errors.password?.type === 'required'} id="password" type="password" {...field} />}
                />
                <Label for="password">
                    Password*
                </Label>
                {errors.password &&
                    <FormFeedback>
                        {errors.password.message}
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
                <ErrorMessage error={error} />
            }
        </Form>
    )
}