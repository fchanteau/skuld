import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { api } from "./api";
import { authSlice } from "@/features/auth/auth.slice";

const rootReducer = combineReducers({
    [api.reducerPath]: api.reducer,
    auth: authSlice.reducer
});

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(api.middleware),
});

export type AppState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;
