import fs from 'fs';

const LOG = 'debug-19ef33.log';
const BASE = process.env.API_BASE || 'http://localhost:5071/api';
const classId = process.env.CLASS_ID || 'L001';

const log = (hypothesisId, message, data) => {
  const o = {
    sessionId: '19ef33',
    runId: 'uc18-2',
    hypothesisId,
    location: 'probe-uc18-2.mjs',
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
    parsed = t.slice(0, 200);
  }
  return { status: r.status, body: parsed };
}

const week = String(process.env.WEEK || '2');
const created = await req('POST', '/diaries', {
  userId: 'sv_001',
  classId,
  week,
  status: 'Submitted',
  taskDescription: 'Hoàn thành module đăng nhập UC-18.2 probe',
  newKnowledge: 'Vue expand/collapse + mark read',
  completionLevel: 8,
  feeling: 4,
  nextPlan: 'Viết UC-18.3 phản hồi',
  issues: 'Không có',
  solutions: 'N/A',
});
log('H1', 'create diary', {
  status: created.status,
  id: created.body?.diary?.id,
  week: created.body?.diary?.week,
});

const diaryId = created.body?.diary?.id;
if (!diaryId) process.exit(1);

const before = await req('GET', `/diaries?classId=${encodeURIComponent(classId)}&userId=sv_001`);
const item = (before.body || []).find((d) => d.id === diaryId);
log('H2', 'before mark-read', {
  status: before.status,
  isReadByTeacher: item?.isReadByTeacher,
});

const marked = await req('PUT', `/diaries/${diaryId}/read?by=teacher`);
log('H3', 'mark read', {
  status: marked.status,
  isReadByTeacher: marked.body?.isReadByTeacher,
});

const fb = await req('PUT', `/diaries/${diaryId}/feedback`, {
  teacherName: 'ThS. Lê Hoàng Nam',
  content: 'Nội dung rõ, tiếp tục duy trì tiến độ.',
});
log('H4', 'add feedback', { status: fb.status, message: fb.body?.message });

const after = await req('GET', `/diaries?classId=${encodeURIComponent(classId)}&userId=sv_001`);
const again = (after.body || []).find((d) => d.id === diaryId);
log('H5', 'detail payload', {
  status: after.status,
  isReadByTeacher: again?.isReadByTeacher,
  feedbacks: again?.feedbacks?.length || 0,
  hasTask: !!again?.taskDescription,
  hasNextPlan: !!again?.nextPlan,
});
