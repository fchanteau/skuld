import type { AppConfig } from "../config";

declare global {
    interface Window {
        appConfiguration: AppConfig;
    }
}