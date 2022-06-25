import { createAsyncThunk } from "@reduxjs/toolkit";
import { AsyncThunkApi } from "../store";

export const login: any = createAsyncThunk<
    void,
    { email: string; password: string }, AsyncThunkApi>
    (`users/login`, async (payload, { getState, dispatch, extra: { api, actionCreators } }) => {
        console.log('FCU payload ', payload)
        const tokenInfos = await api.users.login({
            email: payload.email,
            password: payload.password
        });

        sessionStorage.setItem('token', tokenInfos.token);
        sessionStorage.setItem('refreshToken', tokenInfos.refreshToken);
        
        dispatch(actionCreators.users.fetchUserData());
    },
);

export const fetchUserData: any = createAsyncThunk<void, void, AsyncThunkApi>(
    'users/me',
    async (_, { getState, dispatch, extra: { api, actionCreators } }) => {
        const userData = await api.users.me();
        dispatch(actionCreators.users.setUser(userData));
    }
)