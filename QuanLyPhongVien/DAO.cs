using QuanLyPhongVien.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyPhongVien
{
    class DAO
    {
        private static DAO instance;
        private object command;
        internal static DAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new DAO();
                return instance;
            }
        }
        private DAO() { }
        public DataTable Xem()
        {
            string sql = "SELECT * FROM PhongVien";
            return DataProvider.Instance.execSql(sql);
        }

        public bool LuuPVTS(PVToaSoan ts)
        {
            string sql = "INSERT INTO PhongVien(maPV, tenPV, GT, soDT, ngayVL, gioLT, loaiPV, Luong)" + "VALUES ( @maPV, @tenPV, @GT, @soDT, @ngayVL, @gioLT, @loaiPV, @Luong )";
            Object[] prms = new object[] { ts.id, ts.name, ts.gender, ts.phone, ts.dayWork, ts.LamThemGio, ts.LoaiPV, ts.Luong };
            return DataProvider.Instance.execNonSql(sql, prms) > 0;
        }
        public bool LuuPVTT(PVTT tt)
        {
            string sql = "INSERT INTO PhongVien(maPV, tenPV, GT, soDT, ngayVL, PC, loaiPV, Luong)" + "VALUES ( @maPV, @tenPV, @GT, @soDT, @ngayVL, @PC, @loaiPV, @Luong )";
            Object[] prms = new object[] { tt.id, tt.name, tt.gender, tt.phone, tt.dayWork, tt.PhuCap, tt.LoaiPV, tt.Luong };
            return DataProvider.Instance.execNonSql(sql, prms) > 0;
        }

        public DataTable LayThongTinPhongVienTheoMaDon(string id)
        {
            // Viết câu truy vấn SQL để lấy thông tin chi tiết của phim từ cơ sở dữ liệu
            string query = $"SELECT * FROM PhongVien WHERE maPV = '{id}'";

            // Thực hiện truy vấn và trả về đối tượng Phim
            return DataProvider.Instance.execSql(query);
        }

        public bool XoaThongtinTheoMaDon(string madon)
        {
            try
            {
                string query = $"DELETE FROM PhongVien WHERE MaPV = '{madon}'";
                int affectedRows = DataProvider.Instance.execNonSql(query);

                // Kiểm tra số dòng bị ảnh hưởng, nếu lớn hơn 0, xóa thành công
                return affectedRows > 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Không xóa được phóng viên: ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool SuaPVTS(PVToaSoan suapvts, string madon)
        {
            string query = "UPDATE PhongVien SET tenPV = @tenPV, GT = @GT, soDT = @soDT, ngayVL = @ngayVL, PC = @PC, gioLT = @gioLT, loaiPV = @loaiPV, Luong = @Luong" +
                " WHERE maPV = @maPV";
            object[] prms = new object[] { suapvts.name, suapvts.gender, suapvts.phone, suapvts.dayWork, "", suapvts.LamThemGio, suapvts.LoaiPV, suapvts.Luong, madon };
            return DataProvider.Instance.execNonSql(query, prms) > 0;
        }

        public bool SuaPVTT(PVTT suapvtt, string madon)
        {
            string query = "UPDATE PhongVien SET tenPV = @tenPV, GT = @GT, soDT = @soDT, ngayVL = @ngayVL, PC = @PC, gioLT = @gioLT, loaiPV = @loaiPV, Luong = @Luong" +
                " WHERE maPV = @maPV";
            object[] prms = new object[] { suapvtt.name, suapvtt.gender, suapvtt.phone, suapvtt.dayWork, suapvtt.PhuCap, "", suapvtt.LoaiPV, suapvtt.Luong, madon };
            return DataProvider.Instance.execNonSql(query, prms) > 0;
        }

        public DataTable SapXepPhongVien()
        {
            string query = $"SELECT *\r\nFROM PhongVien\r\nORDER BY \r\n    CASE \r\n        WHEN DATEDIFF(YEAR, ngayVL, GETDATE()) >= 1 THEN DATEDIFF(YEAR, ngayVL, GETDATE())\r\n        ELSE 0 \r\n    END DESC,\r\n    tenPV ASC;";
            return DataProvider.Instance.execSql(query);
        }

        public DataTable ThongKe()
        {
            string query = $"SELECT loaiPV, " +
                $"COUNT(*) AS TongSoLuong, " +
                $"SUM(CASE WHEN loaiPV = 'ThuongTru' THEN PC ELSE 0 END) AS TongPhuCap,    " +
                $"SUM(CASE WHEN loaiPV = 'ToaSoan' THEN gioLT ELSE 0 END) AS TongGioLamThem " +
                $"FROM PhongVien " +
                $"GROUP BY loaiPV;";
            return DataProvider.Instance.execSql(query);
        }

    }
}
