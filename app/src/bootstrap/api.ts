import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import { getAppConfig } from '@/config';
import { getToken } from '@/features/auth/auth.service';

const appConfig = getAppConfig();

export const api = createApi({
  reducerPath: 'skuldApi',
  baseQuery: fetchBaseQuery({
    baseUrl: `${appConfig.apiUrl}`,
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
  }),
  endpoints: () => ({}),
});
