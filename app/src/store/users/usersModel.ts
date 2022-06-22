export interface UserInfos {
    userId: number;
    email: string;
    firstName: string;
    lastName: string;
}

export interface TokenInfos {
    token: string;
    refreshToken: string;
}

export interface UserState {
    userInfos: UserInfos | null;
    tokenInfos: TokenInfos | null;
}