import { LoginPayload } from "./auth.model";

import { api } from "@/common/api";

export const authApi = api.injectEndpoints({
  endpoints: (build) => ({
    login: build.mutation<unknown, LoginPayload>({
      query: (payload) => ({
        url: "auth/login",
        method: "POST",
        body: payload,
      }),
    }),
  }),
});

export const { useLoginMutation } = authApi;
