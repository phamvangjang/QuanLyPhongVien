using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyPhongVien
{
    class DataProvider
    {
        string connstr = @"Data Source=DESKTOP-KTEQEC6\SQLEXPRESS;Initial Catalog=db_QuanLyPhongVien;Integrated Security=True;Encrypt=False";
        private static DataProvider instance;
        internal static DataProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }
        }
        public DataProvider() { }
        // INSERT UPDATE DELETE
        // SELECT 
        public DataTable execSql(string sql, params object[] args)
        {
            DataTable dat = new DataTable();

            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                if (args.Length > 0)
                {
                    string[] processSql = sql.Split(' ');
                    List<string> paramlist = new List<string>();
                    foreach (string s in processSql)
                    {
                        if (s.StartsWith("@"))
                        {
                            if (s.EndsWith(","))
                                s.Remove(s.Length - 1);
                            paramlist.Add(s);
                        }
                    }
                    for (int i = 0; i < args.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramlist[i], args[i]);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dat);
                connection.Close();
            }
            return dat;
        }

        // INSERT UPDATE 
        public int execNonSql(string sql, params object[] args)
        {
            int effectedRows;
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                if (args.Length > 0)
                {
                    string[] processSql = sql.Split(' ');
                    List<string> paramlist = new List<string>();
                    foreach (string s in processSql)
                    {
                        if (s.StartsWith("@"))
                        {
                            //if (s.EndsWith(","))
                            //    s.Remove(s.Length - 1);
                            //paramlist.Add(s);
                            paramlist.Add(s.TrimEnd(','));
                        }
                    }
                    if (paramlist.Count == args.Length)
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            command.Parameters.AddWithValue(paramlist[i], args[i]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Handle mismatch in the number of parameters and arguments!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    /*
                    for (int i = 0; i < args.Length; i++)
                    {
                        command.Parameters.AddWithValue(paramlist[i], args[i]);
                    }
                    */
                }
                effectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
            return effectedRows;
        }
    }
}
