import { AppState } from "../store";

export const getUsersState = (state: AppState) => state.users;

export const getUserInfos = (state: AppState) => state.users.userInfos;

export const isConnected = (state: AppState) => state.users.userInfos !== null;