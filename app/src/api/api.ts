import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { getAppConfig } from "../config";
import { AppState } from "../store/store";

const appConfig = getAppConfig();

export const api = createApi({
    reducerPath: 'skuldApi',
    baseQuery: fetchBaseQuery({
        baseUrl: `${appConfig.apiUrl}`,
        prepareHeaders: (headers, {getState}) => {
            // JWT
            const token = (getState() as AppState).users.tokenInfos?.token;
            if (token) {
                headers.set('authorization', `Bearer ${token}`);
            }

            // default headers
            headers.set('Accept', 'application/json');
            headers.set('Content-Type', 'application/json');
            headers.set('Access-Control-Allow-Origin', '*');

            return headers;
        },
    }),
    endpoints: () => ({})
});