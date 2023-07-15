import react from '@vitejs/plugin-react';

import { defineConfig } from 'vite';
import { VitePluginFonts } from 'vite-plugin-fonts';
import { VitePWA, type VitePWAOptions } from 'vite-plugin-pwa';
import svgr from 'vite-plugin-svgr';
import tsconfigPaths from 'vite-tsconfig-paths';

const configPwa: Partial<VitePWAOptions> = {
  registerType: 'prompt',
  includeAssets: ['favicon.svg'],
  manifest: {
    name: 'Table Tennis Hub',
    short_name: 'TTH',
    description: 'Organize your Table Tennis tournaments in one place',
    theme_color: '#142654',
    display: 'standalone',
    scope: '/',
    start_url: '/',
    orientation: 'portrait',
  },
};

// https://vitejs.dev/config/
export default defineConfig({
  envDir: './env/',
  plugins: [
    react(),
    VitePluginFonts({
      google: {
        families: ['Source Sans Pro'],
      },
    }),
    svgr(),
    tsconfigPaths(),
    VitePWA(configPwa),
  ],
});
