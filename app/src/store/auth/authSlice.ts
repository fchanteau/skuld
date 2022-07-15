import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AuthState, TokenInfos } from "./authModels";

export const initialState: AuthState = {
    tokenInfos: null
};

export const usersSlice = createSlice({
    name: 'users',
    initialState,
    reducers: {
        setTokenInfos: (state, { payload }: PayloadAction<TokenInfos>) => {
            state.tokenInfos = {
                token: payload.token,
                refreshToken: payload.refreshToken
            };
        },
    }
})