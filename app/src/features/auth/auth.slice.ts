import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { AuthState, TokenInfos } from "./auth.model";

export const initialState: AuthState = {
    tokenInfos: null
};

export const authSlice = createSlice({
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