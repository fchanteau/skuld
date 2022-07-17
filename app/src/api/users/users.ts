import { TokenInfos, User } from "@/store/auth";
import { api } from "@/api";
import { toTokenInfos, toUser } from "./mapper";

export interface BackendUser {
    userId: number;
    email: string;
    firstName: string;
    lastName: string;
}

export interface BackendTokenInfos {
    token: string;
    refreshToken: string;
}

export type UserLoginPayload = {
    email: string;
    password: string;
}

export type AddUserPayload = Omit<BackendUser, "userId"> & { password: string };

export const usersApi = api.injectEndpoints({
    endpoints: (build) => ({
        login: build.mutation<TokenInfos, UserLoginPayload>({
            query: (payload) => ({
                url: 'users/login',
                method: 'POST',
                body: payload
            }),
            transformResponse: (response: BackendTokenInfos) => toTokenInfos(response)
        }),
        currentUser: build.query<User, void>({
            query: () => ({
                url: 'users/me',
                method: 'GET'
            }),
            transformResponse: (response: BackendUser) => toUser(response)
        })
    }),
});

export const { useLoginMutation, useCurrentUserQuery } = usersApi;