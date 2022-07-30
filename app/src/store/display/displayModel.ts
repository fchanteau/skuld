export interface DisplayState {
    auth: AuthDisplay;
}

export interface AuthDisplay {
    show: boolean;
    isSignIn: boolean;
    isSignUp: boolean;
}