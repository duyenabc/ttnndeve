import fs from 'fs';

const LOG = 'debug-19ef33.log';
const BASE = process.env.API_BASE || 'http://localhost:5071/api';
const classId = process.env.CLASS_ID || 'L001';

const log = (hypothesisId, message, data) => {
  const o = {
    sessionId: '19ef33',
    runId: 'uc18-1',
    hypothesisId,
    location: 'probe-uc18-1.mjs',
    message,
    data,
    timestamp: Date.now(),
  };
  fs.appendFileSync(LOG, JSON.stringify(o) + '\n');
  console.log(JSON.stringify(o));
};

async function get(path) {
  const r = await fetch(`${BASE}${path}`);
  const t = await r.text();
  let body;
  try {
    body = JSON.parse(t);
  } catch {
    body = t.slice(0, 200);
  }
  return { status: r.status, body };
}

const cfg = await get(`/giangvien/classes/${classId}/diary-config`);
log('H1', 'diary-config', {
  status: cfg.status,
  enabled: cfg.body?.isEnabled,
  min: cfg.body?.minPerWeek,
});

const students = await get(`/giangvien/classes/${classId}/students`);
log('H2', 'students', {
  status: students.status,
  count: Array.isArray(students.body) ? students.body.length : -1,
  sample: Array.isArray(students.body) ? students.body[0] : null,
});

const diaries = await get(`/diaries?classId=${encodeURIComponent(classId)}`);
log('H3', 'diaries', {
  status: diaries.status,
  count: Array.isArray(diaries.body) ? diaries.body.length : -1,
});
