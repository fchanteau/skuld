import { Fetch } from "./utils";

export interface UsersApi {
    me(): Promise<BackendUser>;
    login(payload: LoginUserPayload): Promise<BackendTokenInfo>;
}

export interface BackendUser {
    userId: number;
    email: string;
    firstName: string;
    lastName: string;
}

export interface BackendTokenInfo {
    token: string;
    refreshToken: string;
}

export type LoginUserPayload = {
    email: string;
    password: string;
}

export type AddUserPayload = Omit<BackendUser, "userId"> & { password: string };

export const usersApi = (fetch: Fetch): UsersApi => ({
    me: async () => {
        const res = await fetch('/api/users/me');
        return await res.json();
    },
    login: async (payload: LoginUserPayload) => {
        const res = await fetch('/api/users/login', {method: 'POST', body: JSON.stringify(payload)});
        return await res.json();
    }
});