import { api } from '@/bootstrap';

import { type User } from './user.model';

export const userApi = api.injectEndpoints({
  endpoints: (build) => ({
    currentUser: build.query<User, void>({
      query: () => ({
        url: 'user/me',
        method: 'GET',
      }),
    }),
  }),
});

export const { useCurrentUserQuery } = userApi;
