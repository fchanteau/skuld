import { AppState } from "../store";

export const isConnected = (state: AppState) => state.auth.tokenInfos !== null;