import { Controller, type SubmitHandler, useForm } from 'react-hook-form';
import { FormattedMessage } from 'react-intl';
import { useNavigate } from 'react-router-dom';
import { Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from 'reactstrap';
import { zodResolver } from '@hookform/resolvers/zod';

import { actionCreators } from '@/bootstrap';
import { ErrorMessage } from '@/common/components';
import { useAppDispatch } from '@/hooks';

import { useLoginMutation } from '../auth.api';
import { type UserLoginPayload, useUserLoginSchema } from '../auth.model';
import { saveTokenInfos } from '../auth.service';

export function Login() {
  const dispatch = useAppDispatch();
  const [login, { isLoading, error }] = useLoginMutation();
  const navigate = useNavigate();
  const userLoginSchema = useUserLoginSchema();
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<UserLoginPayload>({
    resolver: zodResolver(userLoginSchema),
  });

  const onSubmit: SubmitHandler<UserLoginPayload> = async (data) => {
    const tokenInfos = await login(data).unwrap();

    saveTokenInfos(tokenInfos);

    dispatch(actionCreators.auth.setConnectedUser(true));
    navigate('/');
  };

  return (
    <>
      <h2>
        <FormattedMessage id="auth.login" />
      </h2>
      <Form onSubmit={handleSubmit(onSubmit)}>
        <FormGroup floating>
          <Controller
            name="email"
            control={control}
            render={({ field }) => <Input invalid={errors.email !== undefined} id="email" type="email" {...field} />}
          />
          <Label for="email">
            <FormattedMessage id="auth.email" />*
          </Label>
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
          <Label for="password">
            <FormattedMessage id="auth.password" />*
          </Label>
          {errors.password && <FormFeedback>{errors.password.message}</FormFeedback>}
        </FormGroup>
        <Button color="primary" className="mb-3" block>
          {isLoading ? <Spinner color="light" /> : <FormattedMessage id="auth.login" />}
        </Button>
        {error && <ErrorMessage error={error} />}
      </Form>
    </>
  );
}
