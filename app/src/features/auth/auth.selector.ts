import { AppState } from "@/bootstrap";

export const isConnected = (state: AppState) => state.auth.tokenInfos !== null;