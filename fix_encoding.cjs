const fs = require('fs');
const path = 'src/views/teacher/ClassStudents.vue';
let content = fs.readFileSync(path, 'utf8');

const lines = content.split('\n');

for (let i = 0; i < lines.length; i++) {
  if (lines[i].includes('["STT"')) {
    lines[i] = '        ["STT", "MSSV", "Họ và tên", "Lớp SH", "Doanh nghiệp thực tập", "Vị trí thực tập", "Nhóm", "Trạng thái", "Tiến độ"]';
  }
  if (lines[i].includes('Giáº£ng viÃªn')) {
    lines[i] = '        [`Giảng viên hướng dẫn: TS. Nguyễn Văn A`],';
  }
}

fs.writeFileSync(path, lines.join('\n'), 'utf8');
console.log('Fixed encoding syntax errors');
