import { displaySlice } from "./display/displaySlice";
import { usersSlice } from "./users/usersSlice";
import * as usersThunks from "./users/usersThunk";

export const actionCreators = {
    [usersSlice.name]: {...usersSlice.actions, ...usersThunks},
    [displaySlice.name]: {...displaySlice.actions}
}

export type ActionCreators = typeof actionCreators;