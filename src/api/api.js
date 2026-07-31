import {
  collection,
  getDocs,
  getDoc,
  doc,
  setDoc,
  updateDoc,
  deleteDoc,
  addDoc,
  query,
  where
} from 'firebase/firestore';
import { db } from '../services/firebase';
import { ensureFirestoreInitialData } from '../services/dbInit';

// Ensure seed data is populated in Firestore
ensureFirestoreInitialData();

function generateToken(type, userId) {
  return `${type}_${userId}_${Math.random().toString(36).substring(2)}_${Date.now()}`;
}

/**
 * Client-Side API adapter connecting directly to Firebase Firestore
 */
const api = {
  async get(url, config = {}) {
    const params = config.params || {};

    if (url === '/auth/me') {
      const storedUser = localStorage.getItem('user');
      if (storedUser) {
        const u = JSON.parse(storedUser);
        try {
          const docSnap = await getDoc(doc(db, 'users', String(u.maNguoiDung || u.id)));
          if (docSnap.exists()) {
            const data = docSnap.data();
            return { data: { ...u, ...data } };
          }
        } catch (e) {
          console.warn('[API] Error fetching me profile from Firestore:', e);
        }
        return { data: u };
      }
      return { data: null };
    }

    if (url === '/account/summary') {
      try {
        const snap = await getDocs(collection(db, 'users'));
        let allUsers = snap.docs.map(d => d.data());
        if (params.role) {
          allUsers = allUsers.filter(u => u.vaiTro === params.role);
        }
        return {
          data: {
            tongSo: allUsers.length,
            dangHoatDong: allUsers.filter(u => u.trangThaiTaiKhoan === 'DangHoatDong').length,
            choKichHoat: allUsers.filter(u => u.trangThaiTaiKhoan === 'ChoKichHoat').length,
            biKhoa: allUsers.filter(u => u.trangThaiTaiKhoan === 'BiKhoa').length
          }
        };
      } catch (e) {
        return { data: { tongSo: 0, dangHoatDong: 0, choKichHoat: 0, biKhoa: 0 } };
      }
    }

    if (url === '/account/users') {
      try {
        const snap = await getDocs(collection(db, 'users'));
        let list = snap.docs.map(d => d.data());
        if (params.role) list = list.filter(u => u.vaiTro === params.role);
        if (params.status) list = list.filter(u => u.trangThaiTaiKhoan === params.status);
        if (params.boMon) list = list.filter(u => u.tenBoMon === params.boMon);
        if (params.lopSinhHoat) list = list.filter(u => u.lopSinhHoat === params.lopSinhHoat);
        if (params.search) {
          const s = params.search.toLowerCase();
          list = list.filter(u =>
            (u.hoTen && u.hoTen.toLowerCase().includes(s)) ||
            (u.maDinhDanh && u.maDinhDanh.toLowerCase().includes(s)) ||
            (u.email && u.email.toLowerCase().includes(s))
          );
        }
        const total = list.length;
        const page = parseInt(params.page || 1, 10);
        const pageSize = parseInt(params.pageSize || 25, 10);
        const items = list.slice((page - 1) * pageSize, page * pageSize);
        return { data: { items, total } };
      } catch (e) {
        return { data: { items: [], total: 0 } };
      }
    }

    if (url.startsWith('/account/users/')) {
      const parts = url.split('/');
      const id = parts[3];
      const isAudit = parts[4] === 'audit';
      if (isAudit) {
        try {
          const snap = await getDocs(collection(db, 'auditLogs'));
          const logs = snap.docs.map(d => d.data()).filter(l => String(l.maNguoiDung) === String(id));
          return { data: logs };
        } catch (e) {
          return { data: [] };
        }
      } else if (id) {
        try {
          const docSnap = await getDoc(doc(db, 'users', String(id)));
          if (docSnap.exists()) return { data: docSnap.data() };
        } catch (e) {}
        return { data: null };
      }
    }

    if (url === '/account/requests') {
      try {
        const snap = await getDocs(collection(db, 'requests'));
        let list = snap.docs.map(d => ({ id: d.id, ...d.data() }));
        if (params.status && params.status !== 'all') {
          list = list.filter(r => r.trangThai === params.status);
        }
        if (params.loai) {
          list = list.filter(r => r.loaiYeuCau === params.loai);
        }
        if (params.search) {
          const s = params.search.toLowerCase();
          list = list.filter(r => (r.tenDoiTuong && r.tenDoiTuong.toLowerCase().includes(s)));
        }
        return { data: list };
      } catch (e) {
        return { data: [] };
      }
    }

    if (url === '/giaovu/cycles') {
      try {
        const snap = await getDocs(collection(db, 'cycles'));
        return { data: snap.docs.map(d => d.data()) };
      } catch (e) {
        return { data: [] };
      }
    }

    if (url === '/notifications') {
      try {
        const snap = await getDocs(collection(db, 'notifications'));
        return { data: snap.docs.map(d => ({ id: d.id, ...d.data() })) };
      } catch (e) {
        return { data: [] };
      }
    }

    if (url === '/diaries') {
      try {
        const snap = await getDocs(collection(db, 'diaries'));
        let list = snap.docs.map(d => ({ id: d.id, ...d.data() }));
        if (params.userId) list = list.filter(d => String(d.userId) === String(params.userId));
        if (params.classId) list = list.filter(d => String(d.classId) === String(params.classId));
        if (params.week) list = list.filter(d => String(d.week) === String(params.week));
        return { data: list };
      } catch (e) {
        return { data: [] };
      }
    }

    if (url === '/giangvien/classes') {
      try {
        const snap = await getDocs(collection(db, 'classes'));
        return { data: snap.docs.map(d => ({ id: d.id, ...d.data() })) };
      } catch (e) {
        return { data: [] };
      }
    }

    if (url.startsWith('/giangvien/classes/')) {
      const parts = url.split('/');
      const maLop = parts[3];
      const isDiaryConfig = parts[4] === 'diary-config';

      try {
        const docSnap = await getDoc(doc(db, 'classes', String(maLop)));
        if (docSnap.exists()) {
          const classData = { id: docSnap.id, ...docSnap.data() };
          if (isDiaryConfig) {
            return { data: classData.diaryConfig || null };
          }
          return { data: classData };
        }
      } catch (e) {}
      if (isDiaryConfig) return { data: null };
      return { data: { maLop, tenLop: `Lớp ${maLop}`, soSinhVien: 0 } };
    }

    return { data: {} };
  },

  async post(url, data = {}) {
    if (url === '/auth/login') {
      const { maDinhDanh, matKhau } = data;
      if (!maDinhDanh) throw { response: { data: { message: 'Vui lòng nhập mã định danh hoặc email' } } };
      if (!matKhau) throw { response: { data: { message: 'Vui lòng nhập mật khẩu' } } };

      const input = maDinhDanh.trim().toLowerCase();
      try {
        const snap = await getDocs(collection(db, 'users'));
        const allUsers = snap.docs.map(d => d.data());
        const user = allUsers.find(u =>
          (u.maDinhDanh && u.maDinhDanh.toLowerCase() === input) ||
          (u.email && u.email.toLowerCase() === input)
        );

        if (!user) {
          throw { response: { data: { message: 'Mã định danh hoặc mật khẩu không đúng' } } };
        }
        if (user.trangThaiTaiKhoan === 'BiKhoa') {
          throw { response: { data: { message: 'Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên' } } };
        }
        if (user.matKhau && user.matKhau !== matKhau) {
          throw { response: { data: { message: 'Mã định danh hoặc mật khẩu không đúng' } } };
        }

        const accessToken = generateToken('access', user.maNguoiDung);
        const refreshToken = generateToken('refresh', user.maNguoiDung);
        let redirectTo = '/dashboard';
        if (user.buocDoiMatKhau) redirectTo = '/change-password';
        else if (user.vaiTro === 'Admin') redirectTo = '/admin/accounts';

        const userPayload = {
          maNguoiDung: user.maNguoiDung,
          maDinhDanh: user.maDinhDanh,
          hoTen: user.hoTen,
          email: user.email,
          vaiTro: user.vaiTro,
          trangThaiTaiKhoan: user.trangThaiTaiKhoan,
          buocDoiMatKhau: user.buocDoiMatKhau,
          quyenQuanLyNguoiDung: user.quyenQuanLyNguoiDung || false
        };

        localStorage.setItem('user', JSON.stringify(userPayload));
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);

        return {
          data: {
            accessToken,
            refreshToken,
            redirectTo,
            user: userPayload
          }
        };
      } catch (err) {
        if (err.response) throw err;
        throw { response: { data: { message: 'Lỗi đăng nhập Firestore' } } };
      }
    }

    if (url === '/auth/refresh') {
      const accessToken = generateToken('access', 'user');
      const refreshToken = generateToken('refresh', 'user');
      return { data: { accessToken, refreshToken } };
    }

    if (url === '/auth/logout') {
      localStorage.removeItem('user');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      return { data: { message: 'Đã đăng xuất' } };
    }

    if (url === '/auth/change-password') {
      const { matKhauMoi } = data;
      const stored = localStorage.getItem('user');
      if (stored) {
        const u = JSON.parse(stored);
        u.buocDoiMatKhau = false;
        localStorage.setItem('user', JSON.stringify(u));
        try {
          await updateDoc(doc(db, 'users', String(u.maNguoiDung)), {
            matKhau: matKhauMoi,
            buocDoiMatKhau: false
          });
        } catch (e) {}
      }
      return { data: { message: 'Đổi mật khẩu thành công' } };
    }

    if (url === '/auth/forgot-password') {
      return { data: { message: 'Đã gửi link đặt lại mật khẩu đến email của bạn.' } };
    }

    if (url === '/auth/reset-password') {
      return { data: { message: 'Đặt lại mật khẩu thành công' } };
    }

    if (url === '/account/users') {
      try {
        const newId = Date.now();
        const newUser = {
          maNguoiDung: newId,
          maDinhDanh: data.maDinhDanh,
          hoTen: data.hoTen,
          email: data.email,
          soDienThoai: data.soDienThoai || '',
          vaiTro: data.vaiTro || 'SinhVien',
          trangThaiTaiKhoan: data.capTaiKhoanNgay ? 'ChoKichHoat' : 'Nhap',
          buocDoiMatKhau: true,
          matKhau: data.capTaiKhoanNgay ? 'Test@1234' : null,
          quyenQuanLyNguoiDung: !!data.quyenQuanLyNguoiDung,
          ngayTao: new Date().toISOString()
        };
        await setDoc(doc(db, 'users', String(newId)), newUser);
        return { data: { message: 'Tạo tài khoản thành công', user: newUser } };
      } catch (e) {
        throw { response: { data: { message: 'Không thể tạo tài khoản trên Firestore' } } };
      }
    }

    if (url.includes('/grant')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'users', String(id)), {
          trangThaiTaiKhoan: 'ChoKichHoat',
          matKhau: 'Test@1234'
        });
      } catch (e) {}
      return { data: { message: 'Đã cấp tài khoản thành công' } };
    }

    if (url.includes('/resend-activation')) {
      return { data: { message: 'Đã gửi lại link kích hoạt' } };
    }

    if (url.includes('/lock')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'users', String(id)), { trangThaiTaiKhoan: 'BiKhoa' });
      } catch (e) {}
      return { data: { message: 'Đã khóa tài khoản' } };
    }

    if (url.includes('/unlock')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'users', String(id)), { trangThaiTaiKhoan: 'DangHoatDong' });
      } catch (e) {}
      return { data: { message: 'Đã mở khóa tài khoản' } };
    }

    if (url.includes('/temp-password')) {
      const id = url.split('/')[3];
      const tempPass = 'Temp@' + Math.floor(1000 + Math.random() * 9000);
      try {
        await updateDoc(doc(db, 'users', String(id)), { matKhau: tempPass, buocDoiMatKhau: true });
      } catch (e) {}
      return { data: { matKhauTam: tempPass, message: 'Đã cấp mật khẩu tạm' } };
    }

    if (url === '/account/users/delete-bulk') {
      const { ids = [] } = data;
      for (const id of ids) {
        try {
          await deleteDoc(doc(db, 'users', String(id)));
        } catch (e) {}
      }
      return { data: { message: `Đã xóa ${ids.length} tài khoản` } };
    }

    if (url === '/account/import-preview') {
      const rows = data.rows || [];
      return {
        data: {
          totalRows: rows.length,
          validRows: rows.map((r, i) => ({ ...r, rowNum: i + 1, quyenQuanLyNguoiDung: false })),
          errorRows: [],
          duplicateRows: []
        }
      };
    }

    if (url === '/account/import-confirm') {
      const items = data.items || [];
      for (const item of items) {
        const id = item.maDinhDanh || Date.now() + Math.random();
        await setDoc(doc(db, 'users', String(id)), {
          maNguoiDung: id,
          maDinhDanh: item.maDinhDanh,
          hoTen: item.hoTen,
          email: item.email,
          vaiTro: data.role || 'SinhVien',
          trangThaiTaiKhoan: data.grantNow ? 'ChoKichHoat' : 'Nhap',
          matKhau: data.grantNow ? 'Test@1234' : null,
          ngayTao: new Date().toISOString()
        });
      }
      return { data: { message: `Import thành công ${items.length} tài khoản` } };
    }

    if (url === '/account/requests/create') {
      const id = Date.now();
      await setDoc(doc(db, 'requests', String(id)), {
        maYeuCau: id,
        ...data,
        trangThai: 'ChoXuLy',
        ngayTao: new Date().toISOString()
      });
      return { data: { message: 'Đã gửi yêu cầu thành công' } };
    }

    if (url.startsWith('/account/requests/') && url.endsWith('/approve')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'requests', String(id)), { trangThai: 'DaDuyet' });
      } catch (e) {}
      return { data: { message: 'Đã duyệt yêu cầu' } };
    }

    if (url.startsWith('/account/requests/') && url.endsWith('/reject')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'requests', String(id)), { trangThai: 'TuChoi', lyDoTuChoi: data.lyDo });
      } catch (e) {}
      return { data: { message: 'Đã từ chối yêu cầu' } };
    }

    if (url === '/giaovu/cycles') {
      const id = Date.now();
      const newCycle = {
        maKy: id,
        tenKy: `${data.loaiThucTap} - ${data.hocKy} - ${data.namHoc}`,
        ...data,
        trangThai: data.isPublish ? 'DangDienRa' : 'Nhap'
      };
      await setDoc(doc(db, 'cycles', String(id)), newCycle);
      return { data: { message: 'Khởi tạo kỳ thực tập thành công', cycle: newCycle } };
    }

    if (url === '/notifications/read-all') {
      return { data: { message: 'Đã đánh dấu tất cả thông báo đã đọc' } };
    }

    if (url === '/chatbot/query') {
      const q = (data.question || '').toLowerCase();
      let answer = 'Tôi là AI Assistant Hỗ trợ Thực tập. Hãy đặt câu hỏi về quy trình thực tập, thời hạn hoặc tiến độ.';
      if (q.includes('quy trình') || q.includes('hướng dẫn')) {
        answer = 'Quy trình thực tập gồm 6 bước: 1. Ghi danh lớp -> 2. Khai báo đơn vị -> 3. Đăng ký đề tài -> 4. Viết nhật ký -> 5. Nộp báo cáo -> 6. Nhận điểm số.';
      }
      return { data: { answer } };
    }

    if (url === '/diaries') {
      const id = Date.now();
      const nowStr = new Date().toISOString();
      const newDiary = {
        id: String(id),
        ...data,
        ngayTao: nowStr,
        ngayCapNhat: nowStr
      };
      await setDoc(doc(db, 'diaries', String(id)), newDiary);
      return { data: { message: 'Đã lưu nhật ký', diary: newDiary } };
    }

    return { data: { message: 'Thao tác thành công' } };
  },

  async put(url, data = {}) {
    if (url === '/auth/me') {
      const stored = localStorage.getItem('user');
      if (stored) {
        const u = JSON.parse(stored);
        u.anhDaiDien = data.anhDaiDien;
        localStorage.setItem('user', JSON.stringify(u));
        try {
          await updateDoc(doc(db, 'users', String(u.maNguoiDung)), { anhDaiDien: data.anhDaiDien });
        } catch (e) {}
      }
      return { data: { message: 'Cập nhật hồ sơ thành công' } };
    }

    if (url.includes('/permission')) {
      const id = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'users', String(id)), {
          quyenQuanLyNguoiDung: !!data.quyenQuanLyNguoiDung
        });
      } catch (e) {}
      return { data: { message: 'Cập nhật quyền thành công' } };
    }

    if (url.startsWith('/diaries/') && url.endsWith('/feedback')) {
      const id = url.split('/')[2];
      try {
        const docSnap = await getDoc(doc(db, 'diaries', String(id)));
        if (docSnap.exists()) {
          const currentData = docSnap.data();
          const feedbacks = currentData.feedbacks || [];
          feedbacks.push({
            teacherName: data.teacherName,
            content: data.content,
            timestamp: new Date().toISOString()
          });
          await updateDoc(doc(db, 'diaries', String(id)), { feedbacks, isReadByTeacher: true, ngayCapNhat: new Date().toISOString() });
        }
      } catch (e) {}
      return { data: { message: 'Đã gửi phản hồi' } };
    }

    if (url.startsWith('/diaries/') && url.endsWith('/read')) {
      const id = url.split('/')[2];
      try {
        await updateDoc(doc(db, 'diaries', String(id)), { isReadByTeacher: true });
      } catch (e) {}
      return { data: { message: 'Đã đánh dấu đọc' } };
    }

    if (url.startsWith('/diaries/') && !url.endsWith('/feedback') && !url.endsWith('/read')) {
      const id = url.split('/')[2];
      try {
        data.ngayCapNhat = new Date().toISOString();
        await updateDoc(doc(db, 'diaries', String(id)), data);
      } catch (e) {
        throw { response: { data: { message: 'Lỗi cập nhật nhật ký' } } };
      }
      return { data: { message: 'Đã cập nhật nhật ký' } };
    }

    if (url.startsWith('/giangvien/classes/') && url.endsWith('/diary-config')) {
      const maLop = url.split('/')[3];
      try {
        await updateDoc(doc(db, 'classes', String(maLop)), { diaryConfig: data });
      } catch (e) {}
      return { data: { message: 'Lưu cấu hình nhật ký thành công' } };
    }

    return { data: { message: 'Cập nhật thành công' } };
  },

  async delete(url) {
    if (url.startsWith('/account/users/')) {
      const id = url.split('/')[3];
      try {
        await deleteDoc(doc(db, 'users', String(id)));
      } catch (e) {}
      return { data: { message: 'Đã xóa tài khoản' } };
    }

    if (url.startsWith('/giaovu/cycles/')) {
      const id = url.split('/')[3];
      try {
        await deleteDoc(doc(db, 'cycles', String(id)));
      } catch (e) {}
      return { data: { message: 'Đã xóa kỳ thực tập' } };
    }

    return { data: { message: 'Xóa thành công' } };
  }
};

export default api;
