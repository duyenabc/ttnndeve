import http from 'node:http';
import { fileURLToPath } from 'node:url';
import path from 'node:path';
import handler from 'serve-handler';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const dist = path.join(__dirname, '..', 'dist');
const port = Number(process.env.PORT) || 3000;

const server = http.createServer((request, response) =>
  handler(request, response, {
    public: dist,
    rewrites: [{ source: '**', destination: '/index.html' }],
  })
);

server.listen(port, '0.0.0.0', () => {
  console.log(`[IMS] Serving dist on http://0.0.0.0:${port}`);
});
