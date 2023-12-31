﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyPhongVienEntityFramwork
{
    public partial class Form1 : Form
    {
        db_QuanLyPhongVienEntities3 _db = new db_QuanLyPhongVienEntities3();
        public Form1()
        {
            InitializeComponent();
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
            Reset();
            ResetListView(_db.PhongViens.ToList());
        }

        private void ResetListView(IEnumerable<PhongVien> p)
        {
            lvDSPV.Items.Clear();
            foreach (var pv in p)
            {
                ListViewItem listViewItem = new ListViewItem(pv.maPV);
                listViewItem.SubItems.Add(pv.tenPV);
                if (pv.GT == "Nam")
                {
                    listViewItem.SubItems.Add(pv.GT);
                }
                else
                {
                    listViewItem.SubItems.Add(pv.GT);
                }
                listViewItem.SubItems.Add(pv.ngayVL.Value.ToString("dd/MM/yyyy"));

                int tn = DateTime.Now.Year - pv.ngayVL.Value.Year;
                if (tn >= 5)
                {
                    listViewItem.BackColor = Color.LightGoldenrodYellow;
                }

                lvDSPV.Items.Add(listViewItem);
            }
        }

        private void Reset()
        {
            foreach (ListViewItem item in lvDSPV.Items)
            {
                item.Selected = false;
            }

            txtMaPV.Clear();
            txtHoTen.Clear();
            txtDienthoai.Clear();
            dtNVL.Value = DateTime.Now;
            rdbtnNam.Checked = true;
            rdbtnToasoan.Checked = true;
            txtGioLT.Clear();
            txtPC.Clear();
            txtMaPV.Enabled = true;
            txtMaPV.Focus();
        }

        private void rdbtnToasoan_CheckedChanged(object sender, EventArgs e)
        {
            txtGioLT.Visible = true;
            lblSogioLT.Visible = true;
            txtGioLT.Clear();
            txtPC.Visible = false;
            lblPC.Visible = false;
        }

        private void rdbtnTT_CheckedChanged(object sender, EventArgs e)
        {
            txtGioLT.Visible = false;
            lblSogioLT.Visible = false;
            txtPC.Clear();
            txtPC.Visible = true;
            lblPC.Visible = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private new bool Validate()
        {
            if (string.IsNullOrEmpty(txtMaPV.Text) || string.IsNullOrEmpty(txtHoTen.Text) || string.IsNullOrEmpty(txtMaPV.Text) || string.IsNullOrEmpty(txtDienthoai.Text) || (string.IsNullOrEmpty(txtGioLT.Text) && rdbtnToasoan.Checked) || (string.IsNullOrEmpty(txtPC.Text) && rdbtnTT.Checked))
            {
                MessageBox.Show("Thông tin không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (dtNVL.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtDienthoai.Text.Any(n => !char.IsDigit(n)) || txtDienthoai.Text.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải là một dãy số và có 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtGioLT.Text.Any(n => !char.IsDigit(n)) && rdbtnToasoan.Checked)
            {
                MessageBox.Show("Giờ làm thêm phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtPC.Text.Any(n => !char.IsDigit(n)) && rdbtnTT.Checked)
            {
                MessageBox.Show("Phụ cấp phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                float salary = 0;
                int giolt = 0;
                float pc = 0;
                bool a = _db.PhongViens.Any(b => b.maPV == txtMaPV.Text);
                if (rdbtnToasoan.Checked)
                {
                    salary += (12000000 + float.Parse(txtGioLT.Text) * 100000 * (float)1.5);
                    giolt = int.Parse(txtGioLT.Text);
                }
                else
                {
                    salary += (12000000 + float.Parse(txtPC.Text));
                    pc = float.Parse(txtPC.Text);
                }
                if (a)
                {
                    MessageBox.Show("Mã phóng viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //save to db
                    _db.PhongViens.Add(new PhongVien() { maPV = txtMaPV.Text, tenPV = txtHoTen.Text, GT = rdbtnNam.Checked ? "Nam" : "Nữ", soDT = txtDienthoai.Text, ngayVL = dtNVL.Value, loaiPV = rdbtnToasoan.Checked ? "ToaSoan" : "ThuongTru", gioLT = giolt, PC = pc, Luong = salary });
                    _db.SaveChanges();

                    //ResetListView
                    ResetListView(_db.PhongViens.ToList());
                }
            }
        }

        private void lvDSPV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDSPV.SelectedItems.Count > 0)
            {
                //get id in listview
                string mapv = lvDSPV.SelectedItems[0].SubItems[0].Text;
                txtMaPV.Enabled = false;
                //find in _db if exists ?
                var pv = _db.PhongViens.SingleOrDefault(z => z.maPV == mapv);
                if (pv != null)
                {
                    txtMaPV.Text = pv.maPV.Trim();
                    txtHoTen.Text = pv.tenPV.Trim();
                    if (pv.GT == "Nam")
                    {
                        rdbtnNam.Checked = true;
                    }
                    else
                    {
                        rdbtnNu.Checked = true;
                    }
                    txtDienthoai.Text = pv.soDT.Trim();
                    dtNVL.Value = pv.ngayVL.Value;
                    if (pv.loaiPV == "ToaSoan")
                    {
                        rdbtnToasoan.Checked = true;
                        txtGioLT.Text = pv.gioLT.ToString();
                        txtPC.Clear();
                    }
                    else if (pv.loaiPV == "ThuongTru")
                    {
                        rdbtnTT.Checked = true;
                        txtPC.Text = pv.PC.ToString().Trim();
                        txtGioLT.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin phóng viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                txtMaPV.Enabled = true;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lvDSPV.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa phóng viên đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    int index = lvDSPV.Items.IndexOf(lvDSPV.SelectedItems[0]);
                    string mpv = lvDSPV.SelectedItems[0].SubItems[0].Text.Trim();

                    PhongVien pv = _db.PhongViens.Where(p => p.maPV.Trim() == mpv).SingleOrDefault();
                    _db.PhongViens.Remove(pv);
                    _db.SaveChanges();

                    lvDSPV.Items.Remove(lvDSPV.SelectedItems[0]);

                    if (lvDSPV.Items.Count > 0)
                    {
                        if (index < lvDSPV.Items.Count)
                        {
                            lvDSPV.Items[index].Selected = true;
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else if (lvDSPV.Items.Count == 0)
                    {
                        Reset();
                    }
                }
                MessageBox.Show("Đã xóa phóng viên thành công", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn phóng viên nào để xóa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (lvDSPV.SelectedItems.Count > 0)
            {
                int index = lvDSPV.Items.IndexOf(lvDSPV.SelectedItems[0]);
                string mpv = lvDSPV.SelectedItems[0].SubItems[0].Text.Trim();

                PhongVien pv = _db.PhongViens.Where(p => p.maPV.Trim() == mpv).SingleOrDefault();
                float salary = 0;
                int giolt = 0;
                float pc = 0;
                if (rdbtnToasoan.Checked)
                {
                    salary += (12000000 + float.Parse(txtGioLT.Text) * 100000 * (float)1.5);
                    giolt = int.Parse(txtGioLT.Text);
                }
                else
                {
                    salary += (12000000 + float.Parse(txtPC.Text));
                    pc = float.Parse(txtPC.Text);
                }

                if (Validate())
                {
                    pv.tenPV = txtHoTen.Text;
                    pv.GT = rdbtnNam.Checked ? "Nam" : "Nu";
                    pv.soDT = txtDienthoai.Text;
                    pv.ngayVL = dtNVL.Value;
                    pv.loaiPV = rdbtnToasoan.Checked ? "ToaSoan" : "ThuongTru";
                    pv.gioLT = giolt;
                    pv.PC = pc;
                    pv.Luong = salary;
                    _db.SaveChanges();
                    ResetListView(_db.PhongViens.ToList());
                    MessageBox.Show("Đã sửa phóng viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                lvDSPV.Items[index].Selected = true;
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn phóng viên nào để sửa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            try
            {
                var sort = _db.PhongViens.OrderBy(p => p.ngayVL).ThenBy(p => p.tenPV).ToList();
                ResetListView(sort);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                var s = from pv in _db.PhongViens
                        group pv by pv.loaiPV into g
                        select new
                        {
                            LoaiPV = g.Key,
                            TongSoLuong = g.Count(),
                            TongLuong = g.Sum(p => p.Luong)
                        };

                string message = "Thống kê theo loại phóng viên\n\n";

                foreach (var t in s)
                {
                    if (t.LoaiPV == "ToaSoan")
                        message += $"Phóng viên tại tòa soạn:\n";
                    else
                        message += $"Phóng viên thường trú:\n";
                    message += $"Số lượng: {t.TongSoLuong}\n";
                    message += $"Tổng lương chi: {t.TongLuong:#,#}\n\n";
                }

                MessageBox.Show(message, "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
