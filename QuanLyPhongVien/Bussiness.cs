using QuanLyPhongVien.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyPhongVien
{
    class Bussiness
    {
        private static Bussiness instance;
        internal static Bussiness Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bussiness();
                }
                return instance;
            }
        }

        public Bussiness() { }
        public void Xem(ListView lv)
        {
            foreach (DataRow row in DAO.Instance.Xem().Rows)
            {
                ListViewItem item = new ListViewItem(row["maPV"].ToString());
                item.SubItems.Add(row["tenPV"].ToString());
                item.SubItems.Add(row["GT"].ToString());
                item.SubItems.Add(row["ngayVL"].ToString());
                TimeSpan thamnien = DateTime.Now - DateTime.Parse(row["ngayVL"].ToString());
                int thamNienNgay = (int)thamnien.TotalDays;
                int tn = thamNienNgay / 365;
                if (tn > 5)
                {
                    item.BackColor = Color.LightGoldenrodYellow;
                }
                lv.Items.Add(item); 
            }
        }

        public void Luu(ListView lv)
        {
            Phongvien p = new Phongvien();
            PVTT pVTT = new PVTT();
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                string gt = "";
                if (form1.rdbtnNam.Checked)
                {
                    gt = "Nam";
                }
                else if (form1.rdbtnNu.Checked)
                {
                    gt = "Nữ";
                }
                //show to listview
                string ngayvl = form1.dtNVL.Value.ToShortDateString();
                ListViewItem listViewItem = new ListViewItem(form1.txtMaPV.Text);
                listViewItem.SubItems.Add(form1.txtHoTen.Text);
                listViewItem.SubItems.Add(gt);
                listViewItem.SubItems.Add(ngayvl);

                //save info form object
                p.id = form1.txtMaPV.Text;
                p.name = form1.txtHoTen.Text;
                p.phone = form1.txtDienthoai.Text;
                p.gender = gt;
                p.dayWork = DateTime.Parse(ngayvl);
                TimeSpan thamnien = DateTime.Now - DateTime.Parse(ngayvl);
                int thamNienNgay = (int)thamnien.TotalDays;
                int tn = thamNienNgay / 365;

                //hight light tn
                if (tn > 5)
                {
                    listViewItem.BackColor = Color.LightGoldenrodYellow;
                }
                form1.lvDSPV.Items.Add(listViewItem);

                if (form1.rdbtnToasoan.Checked)
                {
                    p.LoaiPV = "ToaSoan";
                    p.LamThemGio = int.Parse(form1.txtGioLT.Text);
                    p.Luong = 12000000 + (float)1.5 * 100000 * float.Parse(form1.txtGioLT.Text);
                    p.PhuCap = 0;
                }
                else if (form1.rdbtnTT.Checked)
                {
                    p.LoaiPV = "ThuongTru";
                    p.PhuCap = float.Parse(form1.txtPC.Text);
                    p.Luong = 12000000 + float.Parse(form1.txtPC.Text);
                    p.LamThemGio = 0;
                }
                DAO.Instance.LuuPVTS(p);
                MessageBox.Show("Đã thêm phóng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public Phongvien LayThongTinPhongVienTheoMaDon(string mapv)
        {
            Phongvien pv = new Phongvien();
            DataTable dataTable = DAO.Instance.LayThongTinPhongVienTheoMaDon(mapv);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                pv.id = dataRow[0].ToString();
                pv.name = dataRow[1].ToString();
                pv.gender = dataRow[2].ToString();
                pv.phone = dataRow[3].ToString();
                pv.dayWork = DateTime.Parse(dataRow[4].ToString());
                if (dataRow[7].ToString() == "ThuongTru")
                {
                    pv.PhuCap = float.Parse(dataRow[5].ToString());
                    pv.LamThemGio = 0;
                }
                else
                {
                    pv.LamThemGio = int.Parse(dataRow[6].ToString());
                    pv.PhuCap = 0;
                }
                pv.LoaiPV = dataRow[7].ToString();
                pv.Luong = float.Parse(dataRow[8].ToString());
            }
            return pv;
        }

        public bool XoaThongtinTheoMaDon(string maDon)
        {
            return DAO.Instance.XoaThongtinTheoMaDon(maDon);
        }

        public void Sua(ListView listView)
        {
            Phongvien ts = new Phongvien();
            PVTT tt = new PVTT();
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1.lvDSPV.SelectedItems.Count > 0)
            {
                string madon = form1.lvDSPV.SelectedItems[0].Text;
                if (!string.IsNullOrEmpty(madon))
                {
                    string gt = "";
                    string nvl = form1.dtNVL.Value.ToShortDateString();
                    if (form1.rdbtnNam.Checked)
                    {
                        gt = "Nam";
                    }
                    else if (form1.rdbtnNu.Checked)
                    {
                        gt = "Nữ";
                    }
                    ts.id = form1.txtMaPV.Text;
                    ts.name = form1.txtHoTen.Text;
                    ts.gender = gt;
                    ts.phone = form1.txtDienthoai.Text;
                    ts.dayWork = DateTime.Parse(nvl);
                    
                    if (form1.rdbtnToasoan.Checked)
                    {
                        ts.LoaiPV = "ToaSoan";
                        ts.LamThemGio = int.Parse(form1.txtGioLT.Text);
                        ts.Luong = 12000000 + (float)1.5 * 100000 * float.Parse(form1.txtGioLT.Text);
                        ts.PhuCap = 0;
                    }
                    else if (form1.rdbtnTT.Checked)
                    {
                        ts.LoaiPV = "ThuongTru";
                        ts.PhuCap = float.Parse(form1.txtPC.Text);
                        ts.Luong = 12000000 + float.Parse(form1.txtPC.Text);
                        ts.LamThemGio = 0;
                    }
                    DAO.Instance.SuaPVTS(ts, madon);
                    MessageBox.Show("Dữ liệu đã được sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn phong vien nào để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void SapXepPhongVien(ListView listView)
        {
            foreach (DataRow rows in DAO.Instance.SapXepPhongVien().Rows)
            {
                ListViewItem item = new ListViewItem(rows["maPV"].ToString());
                item.SubItems.Add(rows["tenPV"].ToString());
                item.SubItems.Add(rows["GT"].ToString());
                item.SubItems.Add(rows["ngayVL"].ToString());

                TimeSpan thamnien = DateTime.Now - DateTime.Parse(rows["ngayVL"].ToString());
                int thamNienNgay = (int)thamnien.TotalDays;
                int tn = thamNienNgay / 365;
                if (tn > 5)
                {
                    item.BackColor = Color.LightGoldenrodYellow;
                }
                listView.Items.Add(item);
            }
            MessageBox.Show("Đã sắp xếp phóng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ThongKe()
        {
            DataTable dataTable = DAO.Instance.ThongKe();
            DataRow dataRow1 = dataTable.Rows[0];
            int sltt = dataRow1.Field<int>("TongSoLuong");
            double tpc = dataRow1.Field<double>("TongPhuCap");

            DataRow dataRow2 = dataTable.Rows[1];
            int slts = dataRow2.Field<int>("TongSoLuong");
            int tglt = dataRow2.Field<int>("TongGioLamThem");

            MessageBox.Show("Phóng viên toà soạn: " + slts + " (pv)\n" +
                            "Lương chi: " + ((tglt * 100000 * 1.5) + (slts * 12000000)) + " (VND)\n" +
                            "Phóng viên thường trú: " + sltt + "(pv)\n" +
                            "Lương chi: " + ((tpc) + (sltt * 12000000)) + " (VND)\n",
                            "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
