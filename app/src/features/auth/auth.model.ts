import { useFormatMessage } from '@/common/hooks';

import * as z from 'zod';

export interface TokenInfos {
  token: string;
  refreshToken: string;
}

export interface AuthState {
  isConnected: boolean;
}

export const useUserLoginSchema = () => {
  const formatMessage = useFormatMessage();
  const requiredMessage = formatMessage('common.form.required');
  const stringOptions = {
    required_error: requiredMessage,
  };

  return z.object({
    email: z.string(stringOptions).nonempty(requiredMessage).email(formatMessage('auth.email.error')),
    password: z.string(stringOptions).nonempty(requiredMessage),
  });
};

export type UserLoginPayload = z.infer<ReturnType<typeof useUserLoginSchema>>;

export const useUserRegisterSchema = () => {
  const formatMessage = useFormatMessage();
  const requiredMessage = formatMessage('common.form.required');
  const stringOptions = {
    required_error: requiredMessage,
  };

  return z
    .object({
      email: z.string(stringOptions).nonempty(requiredMessage).email(formatMessage('auth.email.error')),
      password: z.string(stringOptions).nonempty(requiredMessage),
      repeatPassword: z.string(stringOptions).nonempty(requiredMessage),
      lastName: z.string(stringOptions).nonempty(requiredMessage),
      firstName: z.string(stringOptions).nonempty(requiredMessage),
    })
    .refine((data) => data.password === data.repeatPassword, {
      message: formatMessage('auth.repeatPassword.error'),
      path: ['repeatPassword'],
    });
};

export type AddUserFormData = z.infer<ReturnType<typeof useUserRegisterSchema>>;
export type AddUserPayload = Omit<AddUserFormData, 'repeatPassword'>;
