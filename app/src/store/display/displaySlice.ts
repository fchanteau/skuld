import { createSlice } from "@reduxjs/toolkit";
import { DisplayState } from "./displayModel";

export const initialState: DisplayState = {
    login: {
        loading: false,
        show: false
    }
}

export const displaySlice = createSlice({
    name: 'display',
    initialState,
    reducers: {
        toggleLogin: (state) => {
            state.login.show = !state.login.show;
        },
        toggleLoginLoading: (state) => {
            state.login.loading = !state.login.loading;
        }
    }
});