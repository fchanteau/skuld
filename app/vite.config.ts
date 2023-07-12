import react from '@vitejs/plugin-react';

import { defineConfig } from 'vite';
import { VitePluginFonts } from 'vite-plugin-fonts';
import svgr from 'vite-plugin-svgr';
import tsconfigPaths from 'vite-tsconfig-paths';

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
  ],
});
