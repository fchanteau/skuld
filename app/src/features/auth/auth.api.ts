import { api } from '@/bootstrap';

import { type AddUserPayload, type TokenInfos, type User, type UserLoginPayload } from './auth.model';

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
    currentUser: build.query<User, void>({
      query: () => ({
        url: 'auth/me',
        method: 'GET',
      }),
    }),
  }),
});

export const { useLoginMutation, useCurrentUserQuery, useAddUserMutation } = authApi;
