import { createSlice } from "@reduxjs/toolkit";
import { DisplayState } from "./displayModel";

export const initialState: DisplayState = {
    auth: {
        show: false,
        isSignIn: false,
        isSignUp: false
    }
}

export const displaySlice = createSlice({
    name: 'display',
    initialState,
    reducers: {
        showSignIn: (state) => {
            state.auth.show = true;
            state.auth.isSignIn = true;
            state.auth.isSignUp = false;
        },
        showSignUp: (state) => {
            state.auth.show = true;
            state.auth.isSignUp = true;
            state.auth.isSignIn = false;
        },
        hideAuth: (state) => {
            state.auth = {
                show: false,
                isSignIn: false,
                isSignUp: false
            };
        }
    }
});