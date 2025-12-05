using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Utility;

namespace WebApplication_attw114.Dal
{
    public class UserInfor
    {
        static SophyEncrypt SE = new SophyEncrypt();
        public static string connectionString = "Data Source=10.224.69.61,8989;Initial Catalog=SwispeDB;User ID=swispe;Password=!System114&";
        public int ADD(Models.UserMember model) {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserInfor_Add", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 50).Value = model.EmpName;
                cmd.Parameters.Add("@BuName", SqlDbType.NVarChar, 50).Value = model.BuName;
                cmd.Parameters.Add("@Avarta", SqlDbType.VarChar, 300).Value = model.Avartar;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@UserID"].Value.ToString());
                cn.Close();
            }
            return id;
        }
        public void DeleteUser(string empno) {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserInfor_DeleteUser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public void AddPeopleGate(string areaname,string gatename,int inc,int outc)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_PeopleGate_Add", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AreaName", SqlDbType.NVarChar, 50).Value = areaname;
                cmd.Parameters.Add("@GateName", SqlDbType.NVarChar, 50).Value = gatename;
                cmd.Parameters.Add("@InCount", SqlDbType.Int, 4).Value = inc;
                cmd.Parameters.Add("@OutCount", SqlDbType.Int, 4).Value = outc;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public Models.UserMember GetModel(string empno)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@EmpNo", SqlDbType.VarChar,20)};
            parameters[0].Value = empno;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserInfor_GetInforByEmpNo", parameters, "ds");
            Models.UserMember model = new Models.UserMember();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.EmpNo = empno;
                model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                model.Avartar= ds.Tables[0].Rows[0]["Avartar"].ToString();
                model.BuName = ds.Tables[0].Rows[0]["BuName"].ToString();
                model.QRImage = ds.Tables[0].Rows[0]["QRImage"].ToString();
                if (ds.Tables[0].Rows[0]["IsDeleted"].ToString() != "")
                {
                    model.IsDeleted = int.Parse(ds.Tables[0].Rows[0]["IsDeleted"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TimeApplication"].ToString() != "")
                {
                    model.TimeApplication = DateTime.Parse(ds.Tables[0].Rows[0]["TimeApplication"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AvartarUpdate"].ToString() != "")
                {
                    model.AvartarUpdate = ds.Tables[0].Rows[0]["AvartarUpdate"].ToString();
                }
                if (ds.Tables[0].Rows[0]["TimeUpdateAvartar"].ToString() != "")
                {
                    model.TimeUpdateAvartar = DateTime.Parse(ds.Tables[0].Rows[0]["TimeUpdateAvartar"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AreaIDDefault"].ToString() != "")
                {
                    model.AreaIDDefault = int.Parse(ds.Tables[0].Rows[0]["AreaIDDefault"].ToString());
                }
                return model;
            }
            else {
                return null;
            }
        }
        public int CheckExists(string empno)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_Userinfor_CheckExists", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return id;
        }
        public void UpdateInfor(string empno,string avarta)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserInfor_UpdateInfor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cmd.Parameters.Add("@Avartar", SqlDbType.VarChar, 100).Value = avarta;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public void UpdateAvarta(string empno, string avarta)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserInfor_UpdateAvarta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cmd.Parameters.Add("@Avartar", SqlDbType.VarChar, 100).Value = avarta;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public DataSet GetList_UpdateAvarta(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserInfor_GetListUpdate", parameters, "ds");
            return ds;
        }
        public void Approve(int userid, int status)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserInfor_Approve", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int, 4).Value = userid;
                cmd.Parameters.Add("@Status", SqlDbType.Int, 4).Value = status;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public DataSet CardDelete_GetList(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_CardDelete_GetList", parameters, "ds");
            return ds;
        }
    }
}