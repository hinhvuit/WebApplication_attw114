using System.Data.SqlClient;
using System.Data.Sql;

namespace WebApplication_attw114.Dal
{
    public class Dal_TestTb
    {
        static DalDB dalDB = new DalDB();
        static string Strconn = dalDB.getDBConnectStr_Certificate();
        public int Add(Models.ModelForTestTable model)
        {
            dalDB.getDBConnectStr_Certificate();
            using (SqlConnection cn = new SqlConnection(Strconn))
            {
                SqlCommand cmd = new SqlCommand("TestTable_Insert",cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Sdt", System.Data.SqlDbType.Int).Value = model.sdt;
                cmd.Parameters.Add("@Mathe", System.Data.SqlDbType.VarChar, 50).Value = model.Mathe;
                cmd.Parameters.Add("@Diachi", System.Data.SqlDbType.VarChar, 50).Value = model.Diachi;
                cmd.Parameters.Add("@Memo", System.Data.SqlDbType.NVarChar, 50).Value = model.Memo;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
                return 1;
        }
    }
}
