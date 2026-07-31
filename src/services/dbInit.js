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
  }
];

const INITIAL_CLASSES = [];

const INITIAL_CYCLES = [];

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

