import { createAsyncThunk } from "@reduxjs/toolkit";
import { BackendTokenInfo } from "../../api/users";
import { AppThunk, AsyncThunkApi } from "../store";

// export const login = (email: string, password: string): AppThunk<Promise<void>> => async (
//     dispatch,
//     _getState,
//     { api }
// ) => {
//     email = "francois.chanteau49@gmail.com";
//     password = "Carapuce49";
//     const tokenInfos = await api.users.login({
//         email,
//         password
//     });
//     sessionStorage.setItem("token", tokenInfos.token);
//     sessionStorage.setItem("refreshToken", tokenInfos.refreshToken);
// }

export const login2: any = createAsyncThunk<
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

export const fetchUserData = (): AppThunk<Promise<void>> => async (
    dispatch,
    _getState,
    { api, actionCreators },
) => {
    const userData = await api.users.me();
    dispatch(actionCreators.users.setUser(userData));
}

export const fetchUserData2: any = createAsyncThunk<void, void, AsyncThunkApi>(
    'users/me',
    async (_, { getState, dispatch, extra: { api, actionCreators } }) => {
        const userData = await api.users.me();
        dispatch(actionCreators.users.setUser(userData));
    }
)