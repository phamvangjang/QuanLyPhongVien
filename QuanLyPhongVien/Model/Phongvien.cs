using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongVien.Model
{
    class Phongvien
    {
        public string id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public DateTime dayWork { get; set; }
        public float PhuCap { get; set; }
        public int LamThemGio { get; set; }
        public string LoaiPV { get; set; }
    }
}
