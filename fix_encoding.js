import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

function decodeUtf8(str) {
  // Convert a corrupted UTF-8 string back to the original UTF-8 string
  // By converting to buffer using 'binary' / 'latin1' encoding and decoding as 'utf-8'
  const buf = Buffer.from(str, 'binary');
  return buf.toString('utf-8');
}

function walkDir(dir, callback) {
  fs.readdirSync(dir).forEach(f => {
    let dirPath = path.join(dir, f);
    let isDirectory = fs.statSync(dirPath).isDirectory();
    isDirectory ? walkDir(dirPath, callback) : callback(dirPath);
  });
}

const targetDirs = [
  path.join(__dirname, 'src', 'views'),
  path.join(__dirname, 'src', 'components')
];

let changedFiles = 0;

targetDirs.forEach(dir => {
  walkDir(dir, filePath => {
    if (filePath.endsWith('.vue')) {
      let content = fs.readFileSync(filePath, 'utf-8');
      if (content.includes('Lá»›p') || content.includes('thá»±c táº­p') || content.includes('cá»§a')) {
        console.log('Fixing file:', filePath);
        
        // We do a controlled regex replace for typical corrupted sequences
        // Because a global binary decode might break legitimately accented characters added later.
        
        let fixedContent = content
          // We can also just decode the whole file, but let's try the whole file first
          // Wait, if I just do Buffer.from(content, 'binary'), it will corrupt normal UTF-8 characters!
          // So we should only replace corrupted words.
          
        const corruptedWords = [
          'Lá»›p', 'cá»§a', 'tÃ´i', 'Sá»± kiá»‡n', 'BÃ i ná»™p', 'CÃ´ng viá»‡c', 'Quáº£n lÃ½',
          'thá»±c táº­p', 'sinh hoáº¡t', 'chÆ°a cÃ³', 'sinh viÃªn', 'nÃ o', 'hoáº·c', 'khÃ´ng',
          'ghi nháº­n', 'trÆ°á» ng há»£p', 'vi pháº¡m', 'quy Ä‘á»‹nh', 'cháº­m', 'bÃ¡o cÃ¡o',
          'Ä‘Ã­ch', 'giáº£ng viÃªn', 'ThÃ´ng tin', 'cÃ¡ nhÃ¢n', 'Há» ', 'vÃ ', 'tÃªn',
          'Doanh nghiá»‡p', 'Vá»‹ trÃ­', 'NhÃ³m', 'Tráº¡ng thÃ¡i', 'Tiáº¿n Ä‘á»™',
          'Danh sÃ¡ch', 'Sinh viÃªn', 'Sá»‘ lÆ°á»£ng', 'BÃ¡o cÃ¡o', 'Báº£ng Ä‘iá»ƒm',
          'Há» c ká»³', 'NÄƒm há» c', 'Xuáº¥t', 'dá»¯ liá»‡u', 'chÃ­nh thá»©c',
          'táº¡i', 'hiá»‡n', 'Ä‘áº§u', 'cuá»‘i', 'hÆ°á»›ng dáº«n'
        ];
        
        for (const bad of corruptedWords) {
          const good = decodeUtf8(bad);
          fixedContent = fixedContent.replaceAll(bad, good);
        }
        
        fs.writeFileSync(filePath, fixedContent, 'utf-8');
        changedFiles++;
      }
    }
  });
});

console.log('Fixed', changedFiles, 'files.');
