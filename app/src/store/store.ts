import { Action, combineReducers, configureStore, ThunkAction } from "@reduxjs/toolkit";
import { createSkuldApi, SkuldApi } from "../api/api";
import { ActionCreators, actionCreators } from "./actions";
import { usersSlice } from "./users/usersSlice";

const rootReducer = combineReducers({
    users: usersSlice.reducer
});

export const createStore = configureStore({
        reducer: rootReducer,
        middleware: (getDefaultMiddleware) =>
            getDefaultMiddleware({
                thunk: {
                    extraArgument: {
                        api: createSkuldApi(),
                        actionCreators
                    } as ThunkExtraArgument
                }
            }).prepend()
    });

export type AppState = ReturnType<typeof rootReducer>;
export type AppThunk<R = void> = ThunkAction<R, AppState, ThunkExtraArgument, Action<string>>;
export interface ThunkExtraArgument {
    api: SkuldApi;
    actionCreators: ActionCreators;
}
export type AsyncThunkApi = {
    state: AppState;
    extra: ThunkExtraArgument;
}

export type AppDispatch = typeof createStore['dispatch'];