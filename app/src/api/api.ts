import { usersApi, UsersApi } from "./users";
import { Fetch } from "./utils";

export interface SkuldApi {
    users: UsersApi;
}

export const createSkuldApi = (fetch: Fetch): SkuldApi => {
    return {
        users: usersApi(fetch)
    };
}