import { AppState } from "../store";

export const getUsersState = (state: AppState) => state.users;

export const getUserInfos = (state: AppState) => state.users.userInfos;