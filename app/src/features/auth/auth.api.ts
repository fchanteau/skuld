import { api } from '@/bootstrap';

import { type AddUserPayload, type TokenInfos, type UserLoginPayload } from './auth.model';

export const authApi = api.injectEndpoints({
  endpoints: (build) => ({
    login: build.mutation<TokenInfos, UserLoginPayload>({
      query: (payload) => ({
        url: 'auth/login',
        method: 'POST',
        body: payload,
      }),
    }),
    addUser: build.mutation<void, AddUserPayload>({
      query: (payload) => ({
        url: 'auth',
        method: 'POST',
        body: payload,
      }),
    }),
  }),
});

export const { useLoginMutation, useAddUserMutation } = authApi;
