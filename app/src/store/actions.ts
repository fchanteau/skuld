import { displaySlice } from "./display/displaySlice";
import { authSlice } from "./auth/authSlice";

export const actionCreators = {
    [authSlice.name]: {...authSlice.actions},
    [displaySlice.name]: {...displaySlice.actions}
}

export type ActionCreators = typeof actionCreators;