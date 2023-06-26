import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { AuthState, TokenInfos } from "./auth.model";
import { isTokenInfosInStorage } from "./auth.service";

export const initialState: AuthState = {
    isConnected: isTokenInfosInStorage()
};

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setConnectedUser: (state, { payload }: PayloadAction<boolean>) => {
            state.isConnected = payload;
        },
    }
})