import { type TokenInfos } from './auth.model';

export const saveTokenInfos = (tokenInfos: TokenInfos): void => {
  sessionStorage.setItem('token', tokenInfos.token);
  sessionStorage.setItem('refresh-token', tokenInfos.refreshToken);
};

export const getToken = (): string | null => {
  return sessionStorage.getItem('token');
};

export const getRefreshToken = (): string | null => {
  return sessionStorage.getItem('refresh-token');
};

export const isTokenInfosInStorage = (): boolean => {
  return getToken() !== null && getRefreshToken() !== null;
};

export const clearStorage = (): void => sessionStorage.clear();
