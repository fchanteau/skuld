import { TokenInfos, User } from "../../store/auth/authModels";
import { BackendTokenInfos, BackendUser } from "./users";

export function toUser(back: BackendUser): User {
    return {
        ...back
    } as User;
}

export function toTokenInfos(back: BackendTokenInfos): TokenInfos {
    return {
        ...back
    } as TokenInfos;
}