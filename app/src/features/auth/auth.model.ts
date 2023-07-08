export interface User {
  userId: number;
  email: string;
  firstName: string;
  lastName: string;
  role: number;
}

export enum Role {
  User = 1,
  Admin = 2,
}

export interface TokenInfos {
  token: string;
  refreshToken: string;
}

export type UserLoginPayload = {
  email: string;
  password: string;
};

export type AddUserPayload = Omit<User, 'userId'> & { password: string };

export interface AuthState {
  isConnected: boolean;
}
