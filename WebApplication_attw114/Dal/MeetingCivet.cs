using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication_attw114.Dal
{
    public class MeetingCivet
    {
        
        public static string StrConn = "Data Source=10.224.24.30,4433;Initial Catalog=CertificateDB;User ID=formsign;Password=1234567Aa";
        public int ADD(Models.MeetingCivet model)
        {
            int ID;
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_MettingCivet_ADD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 300).Value = model.Name;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 100).Value = model.EmpName;
                cmd.Parameters.Add("@TimeStart", SqlDbType.VarChar, 50).Value = model.TimeStart;
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 50).Value = model.Notes;
                cmd.Parameters.Add("@EndTime", SqlDbType.VarChar, 50).Value = model.EndTime;
                cn.Open();
                cmd.ExecuteNonQuery();
                ID = Convert.ToInt32(cmd.Parameters["@ID"].Value.ToString());
                cn.Close();
            }
            return ID;
        }
        public void MeetSign(Models.UserCivetLoginRecord model)
        {
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_UserCivetLoginRecord_ADD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = model.ID;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = model.Type;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 100).Value = model.EmpName;
                cmd.Parameters.Add("@BGName", SqlDbType.NVarChar, 200).Value = model.BGName;
                cmd.Parameters.Add("@ExtInfor", SqlDbType.NVarChar, 200).Value = model.ExtInfor;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public void Update(int id, string qrimage)
        {
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_MettingCivet_UpdateQRImage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@QRImage", SqlDbType.VarChar, 50).Value = qrimage;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public List<Models.MeetingCivet> GetModel(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_MettingCivet_GetModel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            List<Models.MeetingCivet> listmodel=new List<Models.MeetingCivet>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Models.MeetingCivet model = new Models.MeetingCivet();
                model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                model.Name = dt.Rows[0]["Name"].ToString();
                model.EmpNo = dt.Rows[0]["EmpNo"].ToString();
                model.EmpName = dt.Rows[0]["EmpName"].ToString();
                model.TimeStart = dt.Rows[0]["TimeStart"].ToString();
                model.EndTime = dt.Rows[0]["EndTime"].ToString();
                model.QRImage = dt.Rows[0]["QRImage"].ToString();
                model.Notes = dt.Rows[0]["Notes"].ToString();
                model.TimeApplication = Convert.ToDateTime((dt.Rows[0]["TimeApplication"].ToString()));
                listmodel.Add(model);
            }
            
            return listmodel;
        }
        public Models.MeetingCivet GetModelSigle(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_MettingCivet_GetModel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            if (dt.Rows.Count > 0)
            {
                Models.MeetingCivet model = new Models.MeetingCivet();
                model.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                model.Name = dt.Rows[0]["Name"].ToString();
                model.EmpNo = dt.Rows[0]["EmpNo"].ToString();
                model.EmpName = dt.Rows[0]["EmpName"].ToString();
                model.TimeStart = dt.Rows[0]["TimeStart"].ToString();
                model.EndTime = dt.Rows[0]["EndTime"].ToString();
                model.QRImage = dt.Rows[0]["QRImage"].ToString();
                model.Notes = dt.Rows[0]["Notes"].ToString();
                model.TimeApplication = Convert.ToDateTime((dt.Rows[0]["TimeApplication"].ToString()));

                return model;
            }
            else {
                return null;
            }
        }
        public DataTable UserCivetLoginRecord_GetList(int ID, int type)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_UserCivetLoginRecord_GetList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            return dt;
        }
        public DataTable UP_CivetMeeting_Getinfor(string empno)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_CivetMeeting_Getinfor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar,20).Value = empno;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            return dt;
        }
        public DataTable UserCivetLoginRecord_GetAllCount(int ID, int type)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_UserCivetLoginRecord_GetAllCount", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            return dt;
        }
        public int CheckTime(int id,int type)
        {
            int Reval ;
            using (SqlConnection cn = new SqlConnection(StrConn))
            {
                SqlCommand cmd = new SqlCommand("UP_MettingCivet_Check", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                Reval = Convert.ToInt32(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return Reval;
        }
    }
}
