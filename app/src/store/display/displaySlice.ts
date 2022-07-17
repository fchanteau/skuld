import { createSlice } from "@reduxjs/toolkit";
import { DisplayState } from "./displayModel";

export const initialState: DisplayState = {
    auth: {
        show: false,
        isLogin: true
    }
}

export const displaySlice = createSlice({
    name: 'display',
    initialState,
    reducers: {
        toggleAuth: (state) => {
            state.auth.show = !state.auth.show;
        },
        toggleAuthLogin: (state) => {
            state.auth.isLogin = !state.auth.isLogin;
        }
    }
});