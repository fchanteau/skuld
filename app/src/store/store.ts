import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { api } from "../api";
import { displaySlice } from "./display/displaySlice";
import { usersSlice } from "./auth/authSlice";

const rootReducer = combineReducers({
    [api.reducerPath]: api.reducer,
    users: usersSlice.reducer,
    display: displaySlice.reducer,
});

export const createStore = configureStore({
    reducer: {
        [api.reducerPath]: api.reducer,
        users: usersSlice.reducer,
        display: displaySlice.reducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(api.middleware),
});

export type AppState = ReturnType<typeof createStore.getState>;
export type AppDispatch = typeof createStore.dispatch;