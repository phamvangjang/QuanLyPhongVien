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
                    item.BackColor = Color.Yellow;
                }
                lv.Items.Add(item); 
            }
        }

        public void Luu(ListView lv)
        {
            PVToaSoan pVToaSoan = new PVToaSoan();
            PVTT pVTT = new PVTT();
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                if (form1.rdbtnToasoan.Checked)
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
                    string ngayvl = form1.dtNVL.Value.ToShortDateString();
                    ListViewItem listViewItem = new ListViewItem(form1.txtMaPV.Text);
                    listViewItem.SubItems.Add(form1.txtHoTen.Text);
                    listViewItem.SubItems.Add(gt);
                    listViewItem.SubItems.Add(ngayvl);

                    pVToaSoan.id = form1.txtMaPV.Text;
                    pVToaSoan.name = form1.txtHoTen.Text;
                    pVToaSoan.phone = form1.txtDienthoai.Text;
                    pVToaSoan.gender = gt;
                    pVToaSoan.dayWork = DateTime.Parse(ngayvl);
                    TimeSpan thamnien = DateTime.Now - DateTime.Parse(ngayvl);
                    int thamNienNgay = (int)thamnien.TotalDays;
                    int tn = thamNienNgay / 365;

                    string loaipv = "ToaSoan";
                    pVToaSoan.LoaiPV = loaipv;
                    pVToaSoan.LamThemGio = int.Parse(form1.txtGioLT.Text);

                    if (tn>5)
                    {
                        listViewItem.BackColor = Color.Yellow;
                    }
                    form1.lvDSPV.Items.Add(listViewItem);

                    DAO.Instance.LuuPVTS(pVToaSoan);
                    MessageBox.Show("Đã thêm phóng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (form1.rdbtnTT.Checked)
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
                    string ngayvl = form1.dtNVL.Value.ToShortDateString();
                    ListViewItem listViewItem = new ListViewItem(form1.txtMaPV.Text);
                    listViewItem.SubItems.Add(form1.txtHoTen.Text);
                    listViewItem.SubItems.Add(gt);
                    listViewItem.SubItems.Add(ngayvl);

                    pVTT.id = form1.txtMaPV.Text;
                    pVTT.name = form1.txtHoTen.Text;
                    pVTT.phone = form1.txtDienthoai.Text;
                    pVTT.gender = gt;
                    pVTT.dayWork = DateTime.Parse(ngayvl);
                    TimeSpan thamnien = DateTime.Now - DateTime.Parse(ngayvl);
                    int thamNienNgay = (int)thamnien.TotalDays;
                    int tn = thamNienNgay / 365;

                    string loaipv = "ThuongTru";
                    pVTT.LoaiPV = loaipv;
                    pVTT.PhuCap = float.Parse(form1.txtPC.Text);

                    if (tn > 5)
                    {
                        listViewItem.BackColor = Color.Yellow;
                    }
                    form1.lvDSPV.Items.Add(listViewItem);

                    DAO.Instance.LuuPVTT(pVTT);
                    MessageBox.Show("Đã thêm phóng viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                if (dataRow[5].ToString() != "")
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
            }
            return pv;
        }

        public bool XoaThongtinTheoMaDon(string maDon)
        {
            return DAO.Instance.XoaThongtinTheoMaDon(maDon);
        }

        public void Sua(ListView listView)
        {
            PVToaSoan ts = new PVToaSoan();
            PVTT tt = new PVTT();
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1.lvDSPV.SelectedItems.Count > 0)
            {
                string madon = form1.lvDSPV.SelectedItems[0].Text;
                if (!string.IsNullOrEmpty(madon))
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
                    string nvl = form1.dtNVL.Value.ToShortDateString();
                    if (form1.rdbtnToasoan.Checked)
                    {
                        ts.id = form1.txtMaPV.Text;
                        ts.name = form1.txtHoTen.Text;
                        ts.gender = gt;
                        ts.phone = form1.txtDienthoai.Text;
                        ts.dayWork = DateTime.Parse(nvl);
                        ts.LamThemGio = int.Parse(form1.txtGioLT.Text);
                        ts.LoaiPV = "ToaSoan";
                        DAO.Instance.SuaPVTS(ts, madon);
                        MessageBox.Show("Dữ liệu đã được sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (form1.rdbtnTT.Checked)
                    {
                        tt.id = form1.txtMaPV.Text;
                        tt.name = form1.txtHoTen.Text;
                        tt.gender = gt;
                        tt.phone = form1.txtDienthoai.Text;
                        tt.dayWork = DateTime.Parse(nvl);
                        tt.PhuCap = float.Parse(form1.txtPC.Text);
                        tt.LoaiPV = "ToaSoan";
                        DAO.Instance.SuaPVTT(tt, madon);
                        MessageBox.Show("Dữ liệu đã được sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

            MessageBox.Show("Số lượng phóng viên toà soạn: " + slts + " (phong vien)\n" +
                            "Lương chi cho phóng viên tòa soạn: " + ((tglt * 100000 * 1.5) + (slts * 12000000)) + " (VND)\n" +
                            "Số lượng phóng viên thường trú: " + sltt + "(phong vien)\n" +
                            "Lương chi cho phóng viên thường trú: " + ((tpc) + (sltt * 12000000)) + " (VND)\n",
                            "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
