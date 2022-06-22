import { getAppConfig } from "../config";

export type Fetch = typeof window['fetch'];

const appConfig = getAppConfig();

export const fetch: Fetch = (input, init) => {
    const routeUrl = typeof input === 'string' ? input : (input as Request).url;
    const token = sessionStorage.getItem('token');
    console.log("here")
    const requestInit: RequestInit = {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            ...(token !== null ? { Authorization: `Bearer ${token}` } : {}),
            ...init?.headers
        },
        
        ...init
    }
    return window.fetch(appConfig.apiUrl + routeUrl, requestInit);
}