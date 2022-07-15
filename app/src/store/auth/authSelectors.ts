import { AppState } from "../store";

export const isConnected = (state: AppState) => state.users.tokenInfos !== null;