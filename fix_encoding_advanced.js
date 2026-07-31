import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

function decodeUtf8(str) {
  return Buffer.from(str, 'binary').toString('utf-8');
}

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

targetDirs.forEach(dir => {
  walkDir(dir, filePath => {
    if (filePath.endsWith('.vue')) {
      let content = fs.readFileSync(filePath, 'utf-8');
      
      // Find all words containing typical Windows-1252 mapped characters from UTF-8 Vietnamese
      // e.g. Ã, á, Ä, Æ, â, Ãª, etc.
      const regex = /([a-zA-Z]*[ÃáÄÆâ][\x80-\xFF\w]*)/g;
      
      let modified = false;
      const matches = [...new Set(content.match(regex) || [])];
      
      for (const bad of matches) {
        // Try decoding it
        try {
          const good = decodeUtf8(bad);
          // Only replace if the decoded string looks like a valid Vietnamese word (no replacement char )
          if (!good.includes('') && good !== bad) {
            content = content.replaceAll(bad, good);
            modified = true;
          }
        } catch (e) {}
      }
      
      // Some multi-word sequences like "Há»  vÃ tÃªn" might not be matched as single words by \w
      // Let's just do a more aggressive regex for anything with these chars:
      const phraseRegex = /([a-zA-ZÃáÄÆâ\x80-\xFF\s]{2,})/g;
      const phraseMatches = [...new Set(content.match(phraseRegex) || [])];
      for (const bad of phraseMatches) {
        if (/[ÃáÄÆ]/.test(bad)) {
           try {
             const good = decodeUtf8(bad);
             if (!good.includes('') && good !== bad && /[a-zA-Z]/.test(good)) {
                // To avoid breaking layout, we only replace exactly
                content = content.split(bad).join(good);
                modified = true;
             }
           } catch(e) {}
        }
      }

      if (modified) {
        console.log('Fixed file:', filePath);
        fs.writeFileSync(filePath, content, 'utf-8');
        changedFiles++;
      }
    }
  });
});

console.log('Fixed', changedFiles, 'files.');
