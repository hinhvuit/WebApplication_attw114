using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Utility;

namespace WebApplication_attw114.Dal
{
    public class UserFromECard
    {
        static SophyEncrypt SE = new SophyEncrypt();
        public static string connectionString = "Data Source=10.224.69.61,8989;Initial Catalog=SwispeDB;User ID=swispe;Password=!System114&";
        public void Add(Models.UserFromECard model)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserFromECard_ADD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Card_No", SqlDbType.VarChar, 20).Value = model.Card_No;
                cmd.Parameters.Add("@Emp_Name", SqlDbType.NVarChar, 50).Value = model.Emp_Name;
                cmd.Parameters.Add("@Emp_No", SqlDbType.VarChar, 50).Value = model.Emp_No;
                cmd.Parameters.Add("@Photo", SqlDbType.VarChar, 200).Value = model.Photo;
                cmd.Parameters.Add("@Types", SqlDbType.VarChar, 50).Value = model.Types;
                cmd.Parameters.Add("@Dept", SqlDbType.NVarChar, 200).Value = model.Dept;
                cmd.Parameters.Add("@GRP", SqlDbType.VarChar, 50).Value = model.GRP;
                cmd.Parameters.Add("@TOTALNAME", SqlDbType.NVarChar, 100).Value = model.TOTALNAME;
                cmd.Parameters.Add("@BirthDay", SqlDbType.VarChar, 50).Value = model.BirthDay;
                cmd.Parameters.Add("@DateIn", SqlDbType.VarChar, 50).Value = model.DateIn;
                cmd.Parameters.Add("@IDNo", SqlDbType.VarChar, 50).Value = model.IDNo;
                cmd.Parameters.Add("@BGID", SqlDbType.Int, 4).Value = model.BGID;
                cmd.Parameters.Add("@BBNAME", SqlDbType.VarChar, 50).Value = model.BBNAME;
                cmd.Parameters.Add("@Avartar", SqlDbType.VarChar, 100).Value = model.Avartar;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public DataSet GetListBGGRP(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_BGGRP_GetList", parameters, "ds");
            return ds;
        }
        public DataSet CheckAvarta(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserFromECard_CheckAvarta", parameters, "ds");
            return ds;
        }
        public void UpdateAvarta(string empno,string avartar)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserFromECard_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Emp_No", SqlDbType.VarChar, 20).Value = empno;
                cmd.Parameters.Add("@Avartar", SqlDbType.VarChar, 200).Value = avartar;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public int CheckLocationECard(string empno,int areaid)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserFromECard_CheckPower", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = empno;
                cmd.Parameters.Add("@AreaID", SqlDbType.Int, 4).Value = areaid;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return id;
        }
        public Models.UserFromECard GetModel(string empno)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Emp_No", SqlDbType.VarChar,20)};
            parameters[0].Value = empno;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserFromECard_GetModelByEmpNo", parameters, "ds");
            Models.UserFromECard model = new Models.UserFromECard();
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.Emp_No = empno;
                model.Emp_Name = ds.Tables[0].Rows[0]["Emp_Name"].ToString();
                model.Photo = ds.Tables[0].Rows[0]["Photo"].ToString();
                model.Types = ds.Tables[0].Rows[0]["Types"].ToString();
                model.Dept = ds.Tables[0].Rows[0]["Dept"].ToString();
                model.Avartar = ds.Tables[0].Rows[0]["Avartar"].ToString();
                if (ds.Tables[0].Rows[0]["BGID"].ToString() != "")
                {
                    model.BGID = int.Parse(ds.Tables[0].Rows[0]["BGID"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        public Boolean CheckExists(string empno)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Emp_No", SqlDbType.VarChar,20)};
            parameters[0].Value = empno;
            DataSet ds = DbHelperSQL.RunProcedure("UP_UserFromECard_GetModelByEmpNo", parameters, "ds");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ResetBeforGetInfor(int empno)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserFromECard_ResetBeforGetInfor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int, 4).Value = empno;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public void ResetAffterGetInfor(int empno)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UP_UserFromECard_ResetAffterGetInfor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int, 4).Value = empno;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}