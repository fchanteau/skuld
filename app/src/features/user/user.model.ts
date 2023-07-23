export interface User {
  userId: number;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
}

export enum Role {
  User = 1,
  Admin = 2,
}
