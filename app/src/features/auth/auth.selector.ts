import { type AppState } from '@/bootstrap';

export const isConnected = (state: AppState) => state.auth.isConnected;
