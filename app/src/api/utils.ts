import { getAppConfig } from "../config";

export type Fetch = typeof window['fetch'];

const appConfig = getAppConfig();

export const fetch: Fetch = (input, init) => {
    const routeUrl = typeof input === 'string' ? input : input.url;
    const requestInit: RequestInit = {
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        ...init
    }
    return window.fetch(appConfig.apiUrl + routeUrl, requestInit);
}