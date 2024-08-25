import { combineReducers, configureStore } from "@reduxjs/toolkit";

import { api } from "../api/api";

const rootReducer = combineReducers({
  [api.reducerPath]: api.reducer,
});

// export const actionCreators = {
//   [authSlice.name]: { ...authSlice.actions },
// };

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(api.middleware),
});

export type AppState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;
// export type ActionCreators = typeof actionCreators;
