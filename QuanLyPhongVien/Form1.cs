using QuanLyPhongVien.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyPhongVien
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void grbTTPV_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có muốn đóng ứng dụng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Ngăn chặn việc đóng ứng dụng nếu người dùng không đồng ý.
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rdbtnNam.Checked = true;
            rdbtnToasoan.Checked = true;
            dtNVL.Value = DateTime.Now;
            Bussiness.Instance.Xem(lvDSPV);
        }

        private void rdbtnToasoan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnToasoan.Checked)
            {
                txtGioLT.Visible = true;
                txtPC.Visible = false;
                lblSogioLT.Visible = true;
                lblPC.Visible = false;
            }
        }

        private void rdbtnTT_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnTT.Checked)
            {
                txtGioLT.Visible = false;
                txtPC.Visible = true;
                lblPC.Visible = true;
                lblSogioLT.Visible = false;
            }
        }

        public void Reset()
        {
            // Xóa dữ liệu trong các TextBox nhập liệu
            txtMaPV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtDienthoai.Text = string.Empty;
            rdbtnToasoan.Checked = true;
            rdbtnNam.Checked = true;
            txtGioLT.Text = string.Empty;
            txtPC.Text = string.Empty;
            dtNVL.Value = DateTime.Now;

            // Gán giá trị mặc định cho các TextBox nhập liệu
            txtMaPV.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public bool validate = false;
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin có hợp lệ
            if (string.IsNullOrEmpty(txtMaPV.Text))
            {
                MessageBox.Show("Mã pv không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Tên pv không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDienthoai.Text))
            {
                MessageBox.Show("dien thoai không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime nvl;
            if (!DateTime.TryParse(dtNVL.Text, out nvl))
            {
                MessageBox.Show("Ngày vao lam không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                validate = true;
            }
            if (validate == true)
            {
                Bussiness.Instance.Luu(lvDSPV);
            }
        }

        private void lvDSPV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDSPV.SelectedItems.Count > 0)
            {
                // Lấy giá trị của cột "Mã Đơn" từ dòng được chọn
                string maDon = lvDSPV.SelectedItems[0].SubItems[0].Text;

                // Gọi phương thức trong Bussiness để lấy thông tin chi tiết từ cơ sở dữ liệu
                Phongvien pv = Bussiness.Instance.LayThongTinPhongVienTheoMaDon(maDon);
                txtMaPV.Text = pv.id.ToString();
                txtHoTen.Text = pv.name.ToString();
                txtDienthoai.Text = pv.phone.ToString();
                if (pv.gender == "Nam")
                {
                    rdbtnNam.Checked = true;
                }
                else if (pv.gender == "Nữ")
                {
                    rdbtnNu.Checked = true;
                }
                dtNVL.Text = pv.dayWork.Date.ToString();
                if (pv.LamThemGio == 0)
                {
                    lblSogioLT.Visible = false;
                    txtGioLT.Visible = false;
                    rdbtnTT.Checked = true;
                    rdbtnToasoan.Checked = false;
                    lblPC.Visible = true;
                    txtPC.Visible = true;
                    txtPC.Text = pv.PhuCap.ToString();
                }
                else if (pv.PhuCap == 0)
                {
                    lblPC.Visible = false;
                    txtPC.Visible = false;
                    rdbtnTT.Checked = false;
                    rdbtnToasoan.Checked = true;
                    lblSogioLT.Visible = true;
                    txtGioLT.Visible = true;
                    txtGioLT.Text = pv.LamThemGio.ToString();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lvDSPV.SelectedItems.Count > 0)
            {
                //hien thi hop thoai xac nhan
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc muốn xóa phong vien này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //lay dong duoc chon
                    ListViewItem selectedRow = lvDSPV.SelectedItems[0];
                    // Lấy thông tin của phim từ dòng được chọn
                    string maDon = lvDSPV.SelectedItems[0].SubItems[0].Text;
                    // Lấy index của dòng để xóa
                    int indexToRemove = selectedRow.Index;

                    // Xóa dòng khỏi ListView
                    lvDSPV.Items.Remove(selectedRow);

                    // Chọn dòng liền kề sau nếu có
                    if (indexToRemove < lvDSPV.Items.Count)
                    {
                        lvDSPV.Items[indexToRemove].Selected = true;
                        lvDSPV.Select();
                    }
                    else if (indexToRemove > 0)
                    {
                        // Nếu không có dòng liền kề sau, chọn dòng liền kề trước
                        lvDSPV.Items[indexToRemove - 1].Selected = true;
                        lvDSPV.Select();
                    }
                    else
                    {
                        // Nếu không còn dòng nào trong ListView, xóa thông tin ở bảng điều khiển và đưa trỏ chuột lên txtMaDon
                        Reset();
                    }

                    // Thực hiện xóa từ cơ sở dữ liệu
                    Bussiness.Instance.XoaThongtinTheoMaDon(maDon);
                    MessageBox.Show("Đã xóa phong vien thành công!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn phong vien nào!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Bussiness.Instance.Sua(lvDSPV);
            lvDSPV.Items.Clear();
            Bussiness.Instance.Xem(lvDSPV);
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            lvDSPV.Items.Clear();
            Bussiness.Instance.SapXepPhongVien(lvDSPV);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            Bussiness.Instance.ThongKe();
        }
    }
}
