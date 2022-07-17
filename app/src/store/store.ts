import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { api } from "@/api";
import { displaySlice } from "@/store/display";
import { authSlice } from "@/store/auth";

const rootReducer = combineReducers({
    [api.reducerPath]: api.reducer,
    auth: authSlice.reducer,
    display: displaySlice.reducer,
});

export const createStore = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(api.middleware),
});

export type AppState = ReturnType<typeof createStore.getState>;
export type AppDispatch = typeof createStore.dispatch;