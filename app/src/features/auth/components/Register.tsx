import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { Alert, Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from "reactstrap";
import * as z from 'zod';
 import { zodResolver } from "@hookform/resolvers/zod";
import { useAddUserMutation } from "../auth.api";
import { AddUserPayload } from "../auth.model";
import { ErrorMessage } from "@/common/components";

interface RegisterInput {
    email: string;
    password: string;
    repeatPassword: string;
    lastName: string;
    firstName: string;
}

// const schema: yup.AnyObjectSchema = yup.object({
//     email: yup.string().required("Email is required"),
//     password: yup.string().required("Password is required"),
//     repeatPassword: yup.string().required("Repeat password is required")
//         .oneOf([yup.ref('password'), null], 'Passwords must match'),
//     lastName: yup.string().required("Lastname is required"),
//     firstName: yup.string().required("Firstname is required"),
// })

const schema = z.object({
    email: z.string().min(1).email(),
    password: z.string().min(1),
    repeatPassword: z.string().min(1),
    lastName: z.string().min(1),
    firstName: z.string().min(1)
  })
  .refine((data) => data.password === data.repeatPassword, {
    message: "Passwords don't match",
    path: ["repeatPassword"],
});

export function Register() {
    const [addUser, { isLoading, error, isSuccess }] = useAddUserMutation();
    const { control, handleSubmit, formState: { errors } } = useForm<RegisterInput>({
        resolver: zodResolver(schema)
    });

    const onSubmit: SubmitHandler<RegisterInput> = async data => {
        const payload: AddUserPayload = {
            email: data.email,
            firstName: data.firstName,
            lastName: data.lastName,
            password: data.password
        };
        await addUser(payload);
    };

    return (
        <Form onSubmit={handleSubmit(onSubmit)}>
            <FormGroup floating>
                <Controller
                    name="email"
                    control={control}
                    render={({field}) => <Input invalid={errors.email !== undefined} id="email" type="email" {...field} />}
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
                    name="firstName"
                    control={control}
                    render={({field}) => <Input invalid={errors.firstName !== undefined} id="firstName" type="text" {...field} />}
                />
                <Label for="firstName">
                    Firstname*
                </Label>
                {errors.firstName &&
                    <FormFeedback>
                        {errors.firstName.message}
                    </FormFeedback>
                }
            </FormGroup>
            <FormGroup floating>
                <Controller
                    name="lastName"
                    control={control}
                    render={({field}) => <Input invalid={errors.lastName !== undefined} id="lastName" type="text" {...field} />}
                />
                <Label for="lastName">
                    LastName*
                </Label>
                {errors.lastName &&
                    <FormFeedback>
                        {errors.lastName.message}
                    </FormFeedback>
                }
            </FormGroup>
            <FormGroup floating>
                <Controller
                    name="password"
                    control={control}
                    render={({field}) => <Input invalid={errors.password !== undefined} id="password" type="password" {...field} />}
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
            <FormGroup floating>
                <Controller
                    name="repeatPassword"
                    control={control}
                    render={({field}) => <Input invalid={errors.repeatPassword !== undefined} id="repeatPassword" type="password" {...field} />}
                />
                <Label for="repeatPassword">
                    Repeat password*
                </Label>
                {errors.repeatPassword &&
                    <FormFeedback>
                        {errors.repeatPassword.message}
                    </FormFeedback>
                }
            </FormGroup>
            <Button color="primary" className="w-100 mb-3">
                {
                    isLoading ?
                        <Spinner color="dark" /> :
                        "Create account"
                }
            </Button>

            {error && 
                <ErrorMessage error={error} />
            }

            {isSuccess &&
                <Alert color="success">
                    Account created ! You can now log in the website.
                </Alert>
            }
        </Form>
    )
}