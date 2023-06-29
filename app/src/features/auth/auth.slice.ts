import { createSlice, type PayloadAction } from '@reduxjs/toolkit';

import { type AuthState } from './auth.model';
import { isTokenInfosInStorage } from './auth.service';

export const initialState: AuthState = {
  isConnected: isTokenInfosInStorage(),
};

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setConnectedUser: (state, { payload }: PayloadAction<boolean>) => {
      state.isConnected = payload;
    },
  },
});
