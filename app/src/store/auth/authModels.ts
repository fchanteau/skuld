import { BackendTokenInfos, BackendUser } from "@/api/users";

export type User = BackendUser;
export type TokenInfos = BackendTokenInfos;

export interface AuthState {
    tokenInfos: TokenInfos | null;
}