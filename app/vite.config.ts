import react from '@vitejs/plugin-react';

import { defineConfig } from 'vite';
import { VitePluginFonts } from 'vite-plugin-fonts';
import { VitePWA, type VitePWAOptions } from 'vite-plugin-pwa';
import svgr from 'vite-plugin-svgr';
import tsconfigPaths from 'vite-tsconfig-paths';

const configPwa: Partial<VitePWAOptions> = {
  registerType: 'autoUpdate',
  devOptions: {
    enabled: true,
  },
  includeAssets: ['.src/svgs/tth.svg'],
  manifest: {
    name: 'Table Tennis Hub',
    short_name: 'TTH',
    description: 'Organize your Table Tennis tournaments in one place',
    theme_color: '#ffffff',
    display: 'standalone',
    scope: '/',
    start_url: '/',
    orientation: 'portrait',
    icons: [
      {
        src: 'tth.png', // <== don't add slash, for testing
        sizes: '192x192',
        type: 'image/png',
      },
      {
        src: '/tth.png', // <== don't remove slash, for testing
        sizes: '512x512',
        type: 'image/png',
      },
      {
        src: 'tth.png', // <== don't add slash, for testing
        sizes: '512x512',
        type: 'image/png',
        purpose: 'any maskable',
      },
    ],
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
