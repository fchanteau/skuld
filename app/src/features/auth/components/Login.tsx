import { Controller, type SubmitHandler, useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from 'reactstrap';
import { zodResolver } from '@hookform/resolvers/zod';

import { actionCreators } from '@/bootstrap';
import { ErrorMessage } from '@/common/components';
import { useAppDispatch } from '@/hooks';

import { useLoginMutation } from '../auth.api';
import { type UserLoginPayload } from '../auth.model';
import { saveTokenInfos } from '../auth.service';

import * as z from 'zod';

interface LoginInput {
  email: string;
  password: string;
}

const schema = z.object({
  email: z.string().min(1, { message: 'Email is required' }).email({ message: 'Not an email' }),
  password: z.string().min(1, { message: 'Password is required' }),
});

export function Login() {
  const dispatch = useAppDispatch();
  const [login, { isLoading, error }] = useLoginMutation();
  const navigate = useNavigate();
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginInput>({
    resolver: zodResolver(schema),
  });

  const onSubmit: SubmitHandler<LoginInput> = async (data) => {
    const payload: UserLoginPayload = {
      email: data.email,
      password: data.password,
    };

    const tokenInfos = await login(payload).unwrap();

    saveTokenInfos(tokenInfos);

    dispatch(actionCreators.auth.setConnectedUser(true));
    navigate('/');
  };

  return (
    <>
      <h2>Login</h2>
      <Form onSubmit={handleSubmit(onSubmit)}>
        {JSON.stringify(errors.email)}
        <FormGroup floating>
          <Controller
            name="email"
            control={control}
            render={({ field }) => <Input invalid={errors.email !== undefined} id="email" type="email" {...field} />}
          />
          <Label for="email">Email*</Label>
          {errors.email && <FormFeedback>{errors.email.message}</FormFeedback>}
        </FormGroup>
        <FormGroup floating>
          <Controller
            name="password"
            control={control}
            render={({ field }) => (
              <Input invalid={errors.password !== undefined} id="password" type="password" {...field} />
            )}
          />
          <Label for="password">Password*</Label>
          {errors.password && <FormFeedback>{errors.password.message}</FormFeedback>}
        </FormGroup>
        <Button color="primary" className="mb-3" block>
          {isLoading ? <Spinner color="light" /> : 'Submit'}
        </Button>
        {error && <ErrorMessage error={error} />}
      </Form>
    </>
  );
}
