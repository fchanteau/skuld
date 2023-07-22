import { Controller, type SubmitHandler, useForm } from 'react-hook-form';
import { FormattedMessage } from 'react-intl';
import { Alert, Button, Form, FormFeedback, FormGroup, Input, Label, Spinner } from 'reactstrap';
import { zodResolver } from '@hookform/resolvers/zod';

import { ErrorMessage } from '@/common/components';

import { useAddUserMutation } from '../auth.api';
import { type AddUserFormData, type AddUserPayload, useUserRegisterSchema } from '../auth.model';

export function Register() {
  const [addUser, { isLoading, error, isSuccess }] = useAddUserMutation();
  const userRegisterSchema = useUserRegisterSchema();
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<AddUserFormData>({
    resolver: zodResolver(userRegisterSchema),
  });

  const onSubmit: SubmitHandler<AddUserFormData> = async (data) => {
    const payload: AddUserPayload = {
      email: data.email,
      firstName: data.firstName,
      lastName: data.lastName,
      password: data.password,
    };
    await addUser(payload);
  };

  return (
    <>
      <h2>
        <FormattedMessage id="auth.register" />
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
            name="lastName"
            control={control}
            render={({ field }) => (
              <Input invalid={errors.lastName !== undefined} id="lastName" type="text" {...field} />
            )}
          />
          <Label for="lastName">
            <FormattedMessage id="auth.lastname" />*
          </Label>
          {errors.lastName && <FormFeedback>{errors.lastName.message}</FormFeedback>}
        </FormGroup>
        <FormGroup floating>
          <Controller
            name="firstName"
            control={control}
            render={({ field }) => (
              <Input invalid={errors.firstName !== undefined} id="firstName" type="text" {...field} />
            )}
          />
          <Label for="firstName">
            <FormattedMessage id="auth.firstname" />*
          </Label>
          {errors.firstName && <FormFeedback>{errors.firstName.message}</FormFeedback>}
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
        <FormGroup floating>
          <Controller
            name="repeatPassword"
            control={control}
            render={({ field }) => (
              <Input invalid={errors.repeatPassword !== undefined} id="repeatPassword" type="password" {...field} />
            )}
          />
          <Label for="repeatPassword">
            <FormattedMessage id="auth.repeatPassword" />*
          </Label>
          {errors.repeatPassword && <FormFeedback>{errors.repeatPassword.message}</FormFeedback>}
        </FormGroup>
        <Button color="primary" className="mb-3" block>
          {isLoading ? <Spinner color="light" /> : <FormattedMessage id="auth.register" />}
        </Button>

        {error && <ErrorMessage error={error} />}

        {isSuccess && <Alert color="success">Account created ! You can now log in the website.</Alert>}
      </Form>
    </>
  );
}
