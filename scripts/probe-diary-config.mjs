import fs from 'fs';

const LOG = 'debug-19ef33.log';
const BASE = process.env.API_BASE || 'http://localhost:5071/api';
const classId = process.env.CLASS_ID || 'L001';

const log = (hypothesisId, message, data) => {
  const o = {
    sessionId: '19ef33',
    runId: 'uc14-2',
    hypothesisId,
    location: 'probe-diary-config.mjs',
    message,
    data,
    timestamp: Date.now(),
  };
  fs.appendFileSync(LOG, JSON.stringify(o) + '\n');
  console.log(JSON.stringify(o));
};

const get = async (path) => {
  const r = await fetch(`${BASE}${path}`);
  const t = await r.text();
  let body;
  try {
    body = JSON.parse(t);
  } catch {
    body = t.slice(0, 200);
  }
  return { status: r.status, body };
};

const put = async (path, payload) => {
  const r = await fetch(`${BASE}${path}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
  const t = await r.text();
  let body;
  try {
    body = JSON.parse(t);
  } catch {
    body = t.slice(0, 200);
  }
  return { status: r.status, body };
};

const defaults = await get(`/giangvien/classes/${classId}/diary-config`);
log('H1', 'GET diary-config', {
  status: defaults.status,
  enabled: defaults.body?.isEnabled,
  min: defaults.body?.minPerWeek,
  fields: defaults.body?.fields?.length,
});

const badMin = await put(`/giangvien/classes/${classId}/diary-config`, {
  isEnabled: true,
  minPerWeek: 0,
  deadlineDay: 0,
  deadlineTime: '23:59',
  fields: defaults.body?.fields || [],
});
log('H2a', 'PUT min=0 (expect 400)', { status: badMin.status, message: badMin.body?.message });

const bad = await put(`/giangvien/classes/${classId}/diary-config`, {
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0,
  deadlineTime: '23:59',
  fields: (defaults.body?.fields || []).map((f) => ({ ...f, isEnabled: false })),
});
log('H2b', 'PUT no fields (expect 400)', { status: bad.status, message: bad.body?.message });

const cfg = defaults.body || {};
cfg.isEnabled = true;
cfg.minPerWeek = 3;
cfg.deadlineDay = 0;
cfg.deadlineTime = '23:59';
if (cfg.fields?.length) {
  cfg.fields = cfg.fields.map((f, i) => ({
    ...f,
    isEnabled: true,
    isRequired: i < 2,
  }));
}

const saved = await put(`/giangvien/classes/${classId}/diary-config`, cfg);
log('H3', 'PUT valid', {
  status: saved.status,
  message: saved.body?.message,
  min: saved.body?.config?.minPerWeek,
});

const again = await get(`/giangvien/classes/${classId}/diary-config`);
log('H4', 'GET after save', {
  status: again.status,
  min: again.body?.minPerWeek,
  persisted: again.body?.minPerWeek === 3,
});
