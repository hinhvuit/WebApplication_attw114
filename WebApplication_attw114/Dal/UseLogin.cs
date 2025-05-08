using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace WebApplication_attw114.Dal
{
    public class UseLogin
    {
        private readonly IConfiguration _config;

        string strconn = "Data Source=10.224.24.30,4433;Initial Catalog=CertificateDB;User ID=formsign;Password=1234567Aa";
        public int CheckLoginTest(string EmpNo, string Emp)
        {
            //var strconn = _config["ConnectionStrings:DefaultConnection"];
            
            int retval = 0;
            using (SqlConnection cn = new SqlConnection(strconn.ToString()))
            {
                SqlCommand cmd = new SqlCommand("Att114Manager_TestProc", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@Retval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                retval = Convert.ToInt32(cmd.Parameters["@Retval"].Value.ToString());
                cn.Close();
            }
                return retval;
        }
        public DataTable CheckLogin(string EmpNo, string Passworld)
        {
            //var strconn = _config["ConnectionStrings:DefaultConnection"];
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn.ToString()))
            {
                SqlCommand cmd = new SqlCommand("UP_UserBeforeLoding_loding", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmployeeNo", SqlDbType.VarChar, 50).Value = EmpNo;
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = Passworld;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
                return dt;
        }
    }
}
