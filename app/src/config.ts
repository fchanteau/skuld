export interface AppConfig {
    apiUrl: string;
}

export function getAppConfig(): AppConfig {
    return {
        ...window.appConfiguration
    }
};