import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

function walkDir(dir, callback) {
  fs.readdirSync(dir).forEach(f => {
    let dirPath = path.join(dir, f);
    if (fs.statSync(dirPath).isDirectory()) {
      walkDir(dirPath, callback);
    } else {
      callback(dirPath);
    }
  });
}

const targetDirs = [
  path.join(__dirname, 'src', 'views'),
  path.join(__dirname, 'src', 'components')
];

let changedFiles = 0;

const replacements = {
  'yÃªu cáº§u': 'yêu cầu',
  'chá»‰nh sá»­a': 'chỉnh sửa',
  'sá»­a': 'sửa',
  'Ä‘á»•i': 'đổi',
  'chuáº©n bá»‹': 'chuẩn bị',
  'Duyá»‡t cÃ¡c': 'Duyệt các',
  'tá»± nguyá»‡n': 'tự nguyện',
  'chá»§ Ä‘á»™ng': 'chủ động',
  'táº¡o nhÃ³m má»›i': 'tạo nhóm mới',
  'Gá»­i': 'Gửi',
  'tháº¥t báº¡i': 'thất bại',
  'XÃ¡c Ä‘á»‹nh': 'Xác định',
  'thiáº¿t káº¿': 'thiết kế',
  'giao diá»‡n ngÆ°á» i dÃ¹ng': 'giao diện người dùng',
  'Ä Ã£ cáº­p nháº­t': 'Đã cập nhật',
  'Ä Ã£': 'Đã',
  'TrÆ°á» ng há»£p': 'Trường hợp',
  'CÃ¡c': 'Các',
  'nhÃ³m': 'nhóm',
  'Ä‘Ã£': 'đã'
};

targetDirs.forEach(dir => {
  walkDir(dir, filePath => {
    if (filePath.endsWith('.vue') || filePath.endsWith('.js')) {
      let content = fs.readFileSync(filePath, 'utf-8');
      let originalContent = content;
      
      for (const [bad, good] of Object.entries(replacements)) {
          content = content.replaceAll(bad, good);
      }
      
      if (content !== originalContent) {
          console.log('Fixed', filePath);
          fs.writeFileSync(filePath, content, 'utf-8');
          changedFiles++;
      }
    }
  });
});

console.log('Fixed', changedFiles, 'files.');
