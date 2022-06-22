import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { BackendTokenInfo, BackendUser } from "../../api/users";
import { UserState } from "./usersModel";

export const initialState: UserState = {
    userInfos: null,
    tokenInfos: null
};

export const usersSlice = createSlice({
    name: 'users',
    initialState,
    reducers: {
        setUser: (state, {payload}: PayloadAction<BackendUser>) => {
            state.userInfos = {
                email: payload.email,
                firstName: payload.firstName,
                lastName: payload.lastName,
                userId: payload.userId
            };
        },
        setTokenInfos: (state, {payload}: PayloadAction<BackendTokenInfo>) => {
            state.tokenInfos = {
                token: payload.token,
                refreshToken: payload.refreshToken
            };
        },
    }
})