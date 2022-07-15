import { displaySlice } from "./display/displaySlice";
import { usersSlice } from "./auth/authSlice";

export const actionCreators = {
    [usersSlice.name]: {...usersSlice.actions},
    [displaySlice.name]: {...displaySlice.actions}
}

export type ActionCreators = typeof actionCreators;