import { usersSlice } from "./users/usersSlice";
import * as usersThunks from "./users/usersThunk";

export const actionCreators = {
    [usersSlice.name]: {...usersSlice.actions, ...usersThunks}
}

export type ActionCreators = typeof actionCreators;