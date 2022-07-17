import { displaySlice } from "@/store/display";
import { authSlice } from "@/store/auth";

export const actionCreators = {
    [authSlice.name]: {...authSlice.actions},
    [displaySlice.name]: {...displaySlice.actions}
}

export type ActionCreators = typeof actionCreators;