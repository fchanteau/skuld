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
            state.auth = {
                show: true,
                isSignIn: true,
                isSignUp: false
            };
        },
        showSignUp: (state) => {
            state.auth = {
                show: true,
                isSignIn: false,
                isSignUp: true
            };
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