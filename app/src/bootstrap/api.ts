import {
  type BaseQueryFn,
  createApi,
  type FetchArgs,
  fetchBaseQuery,
  type FetchBaseQueryError,
} from '@reduxjs/toolkit/query/react';

import { actionCreators } from '@/bootstrap';
import { type TokenInfos } from '@/features/auth';
import { clearStorage, getRefreshToken, getToken, saveTokenInfos } from '@/features/auth/auth.service';

import { add } from 'date-fns';
import jwt_decode from 'jwt-decode';

type JwtData = {
  'user.id': string;
  'user.email': string;
  'user.name': string;
  'user.role': string;
  exp: number;
};

const baseQuery = fetchBaseQuery({
  baseUrl: import.meta.env.VITE_APP_API_URL,
  prepareHeaders: (headers) => {
    // JWT
    const token = getToken();
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    // default headers
    headers.set('Accept', 'application/json');
    headers.set('Content-Type', 'application/json');
    headers.set('Access-Control-Allow-Origin', '*');

    return headers;
  },
});

const baseQueryWithReAuth: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (
  args,
  api,
  extraOptions,
) => {
  const token = getToken();

  if (token) {
    const tokenDecoded = jwt_decode<JwtData>(token);

    const expiredDate = new Date(tokenDecoded.exp * 1000);
    const expiredSoon = add(new Date(), { minutes: 1 }) > expiredDate;
    if (expiredSoon) {
      const refreshToken = getRefreshToken() + 'a';
      const refreshRequest: FetchArgs = {
        method: 'POST',
        url: '/auth/refreshtoken',
        body: {
          refreshToken,
        },
      };

      const refreshResult = await baseQuery(refreshRequest, api, extraOptions);

      if (refreshResult.error) {
        api.dispatch(actionCreators.auth.setConnectedUser(false));
        clearStorage();
      } else if (refreshResult.data) {
        saveTokenInfos(refreshResult.data as TokenInfos);
      }
    }
  }
  return await baseQuery(args, api, extraOptions);
};

export const api = createApi({
  reducerPath: 'skuldApi',
  baseQuery: baseQueryWithReAuth,
  endpoints: () => ({}),
});
