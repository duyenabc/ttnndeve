import { collection, getDocs, doc, setDoc } from 'firebase/firestore';
import { db } from './firebase';

const INITIAL_USERS = [
  {
    maNguoiDung: 1,
    maDinhDanh: 'admin',
    hoTen: 'Quản trị viên',
    email: 'admin@due.udn.vn',
    soDienThoai: '0905123456',
    vaiTro: 'Admin',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Admin@123',
    anhDaiDien: null,
    quyenQuanLyNguoiDung: true,
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 2,
    maDinhDanh: 'gvu001',
    hoTen: 'Lê Thị Giáo Vụ',
    email: 'giaovu@due.udn.vn',
    soDienThoai: '0905654321',
    vaiTro: 'GiaoVu',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    quyenQuanLyNguoiDung: true,
    tenKhoa: 'Khoa Thống kê - Tin học',
    tenBoMon: 'Tin học ứng dụng',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 3,
    maDinhDanh: 'gv001',
    hoTen: 'Nguyễn Văn Thành',
    email: 'giangvien@due.udn.vn',
    soDienThoai: '0905112233',
    vaiTro: 'GiangVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    tenBoMon: 'Khoa học dữ liệu',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 4,
    maDinhDanh: 'sv001',
    hoTen: 'Trần Thị Lan',
    email: 'sinhvien@due.udn.vn',
    soDienThoai: '0905998877',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '22CNTT1',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 5,
    maDinhDanh: 'gv002',
    hoTen: 'Phạm Văn Chờ',
    email: 'cho.gv@due.udn.vn',
    soDienThoai: '',
    vaiTro: 'GiangVien',
    trangThaiTaiKhoan: 'ChoKichHoat',
    buocDoiMatKhau: true,
    matKhau: null,
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 14,
    maDinhDanh: 'gv003',
    hoTen: 'Phạm Thị Sạch',
    email: 'sach.gv@due.udn.vn',
    soDienThoai: '0905334455',
    vaiTro: 'GiangVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    tenBoMon: 'Hệ thống thông tin quản lý',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 6,
    maDinhDanh: 'sv002',
    hoTen: 'Hoàng Thị Nháp',
    email: 'nhap.sv@due.udn.vn',
    soDienThoai: '',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'Nhap',
    buocDoiMatKhau: true,
    matKhau: null,
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '22CNTT2',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 7,
    maDinhDanh: 'sv003',
    hoTen: 'Đỗ Văn Khóa',
    email: 'khoa.sv@due.udn.vn',
    soDienThoai: '',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'BiKhoa',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '22CNTT1',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 8,
    maDinhDanh: '22120001',
    hoTen: 'Nguyễn An',
    email: '22120001@student.due.udn.vn',
    soDienThoai: '0901234567',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '22CNTT1',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 9,
    maDinhDanh: '22120002',
    hoTen: 'Trần Bình',
    email: '22120002@student.due.udn.vn',
    soDienThoai: '0901234568',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '22CNTT1',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 10,
    maDinhDanh: '231121521101',
    hoTen: 'Phan Nhật Minh Anh',
    email: '231121521101@student.due.udn.vn',
    soDienThoai: '0905998877',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '49K21.1',
    ngayTao: new Date().toISOString()
  },
  {
    maNguoiDung: 11,
    maDinhDanh: '20241021',
    hoTen: 'Nguyễn Đức Hoàng',
    email: '20241021@student.due.udn.vn',
    soDienThoai: '0905123111',
    vaiTro: 'SinhVien',
    trangThaiTaiKhoan: 'DangHoatDong',
    buocDoiMatKhau: false,
    matKhau: 'Test@1234',
    anhDaiDien: null,
    tenKhoa: 'Khoa Thống kê - Tin học',
    lopSinhHoat: '47K14.1',
    coDeTai: false,
    ngayTao: new Date().toISOString()
  }
];

const defaultDiaryConfig = {
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0, // 0 = Sunday
  deadlineTime: '23:59',
  fields: [
    { id: 'taskDescription', label: 'Mô tả nhiệm vụ đã thực hiện', isEnabled: true, isRequired: true },
    { id: 'newKnowledge', label: 'Kiến thức/kỹ năng mới', isEnabled: true, isRequired: false },
    { id: 'completionLevel', label: 'Mức độ hoàn thành (thang 1-10)', isEnabled: true, isRequired: true },
    { id: 'issues', label: 'Khó khăn/vướng mắc', isEnabled: true, isRequired: false },
    { id: 'solutions', label: 'Cách xử lý/hướng giải quyết', isEnabled: true, isRequired: false },
    { id: 'feedback', label: 'Phản hồi từ người hướng dẫn (nếu có)', isEnabled: true, isRequired: false },
    { id: 'supportNeeded', label: 'Cần GVHD hỗ trợ gì không', isEnabled: true, isRequired: false },
    { id: 'feeling', label: 'Cảm nhận chung (thang 1-5)', isEnabled: true, isRequired: true },
    { id: 'nextPlan', label: 'Kế hoạch làm việc tiếp theo', isEnabled: true, isRequired: true },
    { id: 'proofFile', label: 'Minh chứng công việc', isEnabled: true, isRequired: false },
    { id: 'proofDescription', label: 'Mô tả minh chứng', isEnabled: true, isRequired: false }
  ]
};

const INITIAL_CLASSES = [
  {
    maLop: 101,
    tenLop: 'MIS2012_2025-2026_1',
    maKy: 1,
    tenKy: 'Tốt nghiệp - HK2 - 2025-2026',
    maGiangVien: 3,
    tenGiangVien: 'Nguyễn Văn Thành',
    ghiDanhMo: true,
    hanGhiDanh: '2026-03-15T23:59',
    maThamGia: 'ABC123X',
    soSinhVien: 3,
    trangThaiDangKyDeTai: 'DangMo',
    diaryConfig: defaultDiaryConfig
  },
  {
    maLop: 102,
    tenLop: 'MIS2012_2025-2026_2',
    maKy: 1,
    tenKy: 'Tốt nghiệp - HK2 - 2025-2026',
    maGiangVien: 14,
    tenGiangVien: 'Phạm Thị Sạch',
    ghiDanhMo: true,
    hanGhiDanh: '2026-03-15T23:59',
    maThamGia: 'XYZ789A',
    soSinhVien: 3,
    trangThaiDangKyDeTai: 'DangMo',
    diaryConfig: defaultDiaryConfig
  }
];

const INITIAL_CYCLES = [
  {
    maKy: 1,
    tenKy: 'Tốt nghiệp - HK2 - 2025-2026',
    loaiThucTap: 'Tốt nghiệp',
    hocKy: 'HK2',
    namHoc: '2025-2026',
    ngayBatDau: '2026-02-01',
    ngayKetThuc: '2026-06-30',
    soTuan: 21,
    trangThai: 'DangDienRa',
    khoaSaoDiem: { ngayKhoa: '2026-06-25T23:59', canhBaoSomNgay: 3, daKhoa: false }
  }
];

export async function ensureFirestoreInitialData() {
  try {
    const usersSnap = await getDocs(collection(db, 'users'));
    if (usersSnap.empty) {
      console.log('[dbInit] Initializing Firestore user accounts...');
      for (const u of INITIAL_USERS) {
        await setDoc(doc(db, 'users', String(u.maNguoiDung)), u);
      }
    }

    const classesSnap = await getDocs(collection(db, 'classes'));
    if (classesSnap.empty) {
      for (const c of INITIAL_CLASSES) {
        await setDoc(doc(db, 'classes', String(c.maLop)), c);
      }
    }

    const cyclesSnap = await getDocs(collection(db, 'cycles'));
    if (cyclesSnap.empty) {
      for (const cy of INITIAL_CYCLES) {
        await setDoc(doc(db, 'cycles', String(cy.maKy)), cy);
      }
    }
  } catch (err) {
    console.warn('[dbInit] Error initializing Firestore data:', err);
  }
}
