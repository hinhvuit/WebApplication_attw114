using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Utility;

namespace WebApplication_attw114.Dal
{
    public class UserRecord
    {
        static SophyEncrypt SE = new SophyEncrypt();
        public static string connectionString = "Data Source=10.224.24.30,4433;Initial Catalog=SwispeDB;User ID=swispe;Password=1234567Aa";
        public int Add(Models.UserRecord model)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserRecord_ADD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IDRecord", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 50).Value = model.EmpName;
                cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 100).Value = model.Location;
                cmd.Parameters.Add("@Latitude", SqlDbType.Float).Value = model.Latitude;
                cmd.Parameters.Add("@Longitude", SqlDbType.Float).Value = model.Longitude;
                cmd.Parameters.Add("@Type", SqlDbType.Int, 4).Value = model.Type;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@IDRecord"].Value.ToString());
                cn.Close();
            }
            return id;
        }
        public DataSet GetListAreaDetail(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_AreaLocationDetail_GetList", parameters, "ds");
            return ds;
        }
        public DataSet AreaBGID_GetByEmpNo(string empno)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@EmpNo", SqlDbType.VarChar,20)};
            parameters[0].Value = empno;
            DataSet ds = DbHelperSQL.RunProcedure("Up_AreaBGID_GetByEmpNo", parameters, "ds");
            return ds;
        }
        public Models.UserRecord GetModelByID(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@IDRecord", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserRecord_GetModelByID", parameters, "ds");
            Models.UserRecord model = new Models.UserRecord();
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.IDRecord = id;
                model.EmpNo = ds.Tables[0].Rows[0]["EmpNo"].ToString();
                model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                model.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                model.BuName = ds.Tables[0].Rows[0]["BuName"].ToString();
                model.QRImage = ds.Tables[0].Rows[0]["QRImage"].ToString();
                model.Avarta = ds.Tables[0].Rows[0]["Avarta"].ToString();
                if (ds.Tables[0].Rows[0]["Latitude"].ToString() != "")
                {
                    model.Latitude = float.Parse(ds.Tables[0].Rows[0]["Latitude"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Longitude"].ToString() != "")
                {
                    model.Longitude = float.Parse(ds.Tables[0].Rows[0]["Longitude"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Type"].ToString() != "")
                {
                    model.Type = int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TimeRecord"].ToString() != "")
                {
                    model.TimeRecord = DateTime.Parse(ds.Tables[0].Rows[0]["TimeRecord"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        public DataSet CheckAreaByLaLong(float lati,float longti)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Latitude", SqlDbType.Float),
                    new SqlParameter("@Longtitude", SqlDbType.Float)};
            parameters[0].Value = lati;
            parameters[1].Value = longti;
            DataSet ds = DbHelperSQL.RunProcedure("UP_AreaLocationDetail_CheckAreaByLaLong", parameters, "ds");
            return ds;
        }
        public List<Models.AreaPower> GetAreaPowers(string empno)
        {
            List<Models.AreaPower> RetList = new List<Models.AreaPower>();
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Up_AreaBGID_GetByEmpNo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Models.AreaPower model = new Models.AreaPower();
                model.AreaName = dt.Rows[i]["AreaName"].ToString();
                RetList.Add(model);
            }
            return RetList;
        }
    }
}