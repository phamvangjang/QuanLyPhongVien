﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            /*
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có muốn đóng ứng dụng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Ngăn chặn việc đóng ứng dụng nếu người dùng không đồng ý.
            }
            */
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
                int tn = 0;
                ListViewItem listViewItem = new ListViewItem(pv.maPV);
                tn = DateTime.Now.Year - pv.ngayVL.Value.Year;
                if (tn >= 5)
                {
                    listViewItem.BackColor = Color.Yellow;
                }
                listViewItem.SubItems.Add(pv.tenPV);

                if (pv.GT == "Nam")
                {
                    listViewItem.SubItems.Add(pv.GT);
                }
                else
                {
                    listViewItem.SubItems.Add(pv.GT);
                }
                listViewItem.SubItems.Add(pv.ngayVL.ToString());

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
            txtMaPV.Focus();
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(txtMaPV.Text) || string.IsNullOrEmpty(txtHoTen.Text) || string.IsNullOrEmpty(txtMaPV.Text) || string.IsNullOrEmpty(txtDienthoai.Text) || (string.IsNullOrEmpty(txtGioLT.Text)&&rdbtnToasoan.Checked) || (string.IsNullOrEmpty(txtPC.Text) && rdbtnTT.Checked))
            {
                MessageBox.Show("Thông tin không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (dtNVL.Value>DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtDienthoai.Text.Any(n=>!char.IsDigit(n))||txtDienthoai.Text.Length!=10)
            {
                MessageBox.Show("Số điện thoại phải là một dãy số và có 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtGioLT.Text.Any(n=>!char.IsDigit(n))&&rdbtnToasoan.Checked)
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
                float salary = 12000000;
                bool a = _db.PhongViens.Any(b => b.maPV == txtMaPV.Text);
                if (rdbtnToasoan.Checked)
                {
                    salary += float.Parse(txtGioLT.Text) * 100000 * (float)1.5;
                }
                else
                {
                    salary += float.Parse(txtPC.Text);
                }
                if (a)
                {
                    MessageBox.Show("Mã phóng viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //save to db
                    _db.PhongViens.Add(new PhongVien() { maPV = txtMaPV.Text, tenPV=txtHoTen.Text, soDT=txtDienthoai.Text, GT=rdbtnNam.Checked? "Nam" : "Nữ", ngayVL=dtNVL.Value,loaiPV=rdbtnToasoan.Checked? "ToaSoan" : "ThuongTru", Luong=salary});
                    _db.SaveChanges();

                    //show to listview
                    ListViewItem listViewItem= new ListViewItem(txtMaPV.Text);
                    listViewItem.SubItems.Add(txtHoTen.Text);
                    listViewItem.SubItems.Add(rdbtnNam.Checked?rdbtnNam.Text:rdbtnNu.Text);
                    listViewItem.SubItems.Add(dtNVL.Value.ToString("dd/MM/yyyy"));

                    //hight light listviewitem 
                    int tn = DateTime.Now.Year - dtNVL.Value.Year;
                    lvDSPV.Items.Add(listViewItem);

                    listViewItem.Selected = true;
                    if (tn>=5)
                    {
                        listViewItem.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void lvDSPV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}