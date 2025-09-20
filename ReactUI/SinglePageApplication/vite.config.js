import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    open: true
  }
});
// export default defineConfig({
//   plugins: [react()],
//   server: {
//     proxy: {
//       "/api": {
//         target: "https://localhost:7017",
//         changeOrigin: true,
//         secure: false, // dev cert is self-signed
//         // rewrite "/api/product/GetAll" -> "/product/GetAll"
//         rewrite: (path) => path.replace(/^\/api/, ""),
//       },
//     },
//   },
// });