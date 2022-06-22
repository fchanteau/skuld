import { usersApi, UsersApi } from "./users";
import { fetch } from "./utils";

export interface SkuldApi {
    users: UsersApi;
}

export const createSkuldApi = (): SkuldApi => {
    return {
        users: usersApi(fetch)
    };
}