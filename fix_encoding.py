import re

path = 'src/views/teacher/ClassStudents.vue'
with open(path, 'r', encoding='utf8') as f:
    content = f.read()

# Fix the specific array of columns causing the syntax error
content = re.sub(
    r'\["STT", "MSSV", "Há»  và tên", "L.*?:p SH", "Doanh nghi.*?p thực tập", "V.*? trí thực tập", "Nhóm", "Trạng thái", "Tiến.*?\]',
    '["STT", "MSSV", "Họ và tên", "Lớp SH", "Doanh nghiệp thực tập", "Vị trí thực tập", "Nhóm", "Trạng thái", "Tiến độ"]',
    content
)

# Also fix the giang vien huong dan
content = re.sub(
    r'\[`Giáº£ng viÃªn hư.*?ng dẫn: TS\. Nguyá»…n VÄƒn A`\]',
    '[`Giảng viên hướng dẫn: TS. Nguyễn Văn A`]',
    content
)

with open(path, 'w', encoding='utf8') as f:
    f.write(content)
