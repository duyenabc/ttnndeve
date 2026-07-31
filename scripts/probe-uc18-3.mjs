import fs from 'fs';

const LOG = 'debug-19ef33.log';
const BASE = process.env.API_BASE || 'http://localhost:5071/api';
const classId = process.env.CLASS_ID || 'L001';

const log = (hypothesisId, message, data) => {
  const o = {
    sessionId: '19ef33',
    runId: 'uc18-3',
    hypothesisId,
    location: 'probe-uc18-3.mjs',
    message,
    data,
    timestamp: Date.now(),
  };
  fs.appendFileSync(LOG, JSON.stringify(o) + '\n');
  console.log(JSON.stringify(o));
};

async function req(method, path, body) {
  const r = await fetch(`${BASE}${path}`, {
    method,
    headers: body ? { 'Content-Type': 'application/json' } : undefined,
    body: body ? JSON.stringify(body) : undefined,
  });
  const t = await r.text();
  let parsed;
  try {
    parsed = JSON.parse(t);
  } catch {
    parsed = t.slice(0, 240);
  }
  return { status: r.status, body: parsed };
}

const created = await req('POST', '/diaries', {
  userId: 'sv_001',
  classId,
  week: '2',
  status: 'Submitted',
  taskDescription: 'UC-18.3 probe task',
  nextPlan: 'Next week review',
  completionLevel: 7,
  feeling: 4,
});
const diaryId = created.body?.diary?.id;
log('H1', 'create diary', { status: created.status, diaryId });

const empty = await req('PUT', `/diaries/${diaryId}/feedback`, {
  teacherName: 'ThS. Lê Hoàng Nam',
  content: '   ',
});
log('H2', 'empty feedback (expect 400)', {
  status: empty.status,
  message: empty.body?.message,
});

const ok = await req('PUT', `/diaries/${diaryId}/feedback`, {
  teacherName: 'ThS. Lê Hoàng Nam',
  content: 'Tiến độ tốt, tiếp tục duy trì.',
});
log('H3', 'valid feedback', {
  status: ok.status,
  message: ok.body?.message,
  notification: ok.body?.notification,
  feedbackId: ok.body?.feedback?.id,
});

const again = await req('PUT', `/diaries/${diaryId}/feedback`, {
  teacherName: 'ThS. Lê Hoàng Nam',
  content: 'Nhận xét lần 2 — bổ sung minh chứng tuần sau.',
});
log('H4', 'second feedback (history)', {
  status: again.status,
  feedbackId: again.body?.feedback?.id,
});

const hist = await req(
  'GET',
  `/diaries/feedback-history?userId=sv_001&classId=${encodeURIComponent(classId)}`
);
log('H5', 'feedback-history', {
  status: hist.status,
  count: Array.isArray(hist.body) ? hist.body.length : -1,
  latest: Array.isArray(hist.body) ? hist.body[0]?.content?.slice(0, 60) : null,
});
