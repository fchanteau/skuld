import { authSlice } from "@/features/auth/auth.slice";

export const actionCreators = {
    [authSlice.name]: {...authSlice.actions}
}

export type ActionCreators = typeof actionCreators;