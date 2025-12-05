using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication_attw114.Dal
{
    public class OndutyForAttwSign
    {
        string strconn = "Data Source=10.224.69.61,8989;Initial Catalog=CertificateDB30;User ID=formsign;Password=!System114&";

        public string SaveCodeForTest(string code)
        {
            string retval = "";
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyForAttw_ListLocationsSign_Add", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("", SqlDbType.Int).Value = "";
            }
            return retval;
        }

        public string UpdateCodeTest(string code)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyForAttw_ListLocationsSign_UpdatecodeFortest", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = code;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            return "Cuccess";
        }

        public DataTable getListTest()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyForAttw_GetListHistorySignTest", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpID", SqlDbType.VarChar, 50).Value = "";
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            return dt;
        }

        public List<Models.ListRuleOfOndutyAttw> GetListRulesByLocaIDType(string LocationID, int Type)
        {
            List<Models.ListRuleOfOndutyAttw> RetList = new List<Models.ListRuleOfOndutyAttw>();
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyForAttw_GetListRuleByLocationID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 20).Value = LocationID;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = Type;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            for(int i= 0; i < dt.Rows.Count; i++)
            {
                Models.ListRuleOfOndutyAttw model = new Models.ListRuleOfOndutyAttw();
                model.Stt = Convert.ToInt32(dt.Rows[i]["Stt"].ToString());
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                model.Type = Convert.ToInt32(dt.Rows[i]["Type"].ToString());
                model.RuleDetailName = dt.Rows[i]["RuleDetailName"].ToString();
                RetList.Add(model);
            }
            return RetList;
        }

        public DataTable getInforFormByLocationID(string LocationID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutySignAttw_getInforFormByLocatiID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 50).Value = LocationID;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            return dt;
        }

        public DataTable getInforFormByOndutySignID(int OndutySignID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OnduttyAttw_getInforFormByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OndutyAttwSignID", SqlDbType.Int).Value = OndutySignID;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            return dt;
        }

        public void OndutySign_Sign(Models.ModelForOndutyAttwSign model)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyOfAttwDepart_SignAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 50).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 200).Value = model.EmpName;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 20).Value = model.LocationID;
                cmd.Parameters.Add("@IsOke", SqlDbType.Int).Value = model.Isoke;
                cmd.Parameters.Add("@Image", SqlDbType.VarChar, 50).Value = model.ImageName;
                cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = model.Notes;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public void OndutySign_UpdateCoordinate(string LocationID, float Latitude, float Longitude)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutyAttw_UpdateCoordinate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 20).Value = LocationID;
                cmd.Parameters.Add("@Latitude", SqlDbType.Float).Value = Latitude;
                cmd.Parameters.Add("@Longitide", SqlDbType.Float).Value = Longitude;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public List<Models.Model_HistorySign> getHistorySignByLocationIDNow(string LocationID, string EmpNo)
        {
            DataTable dt = new DataTable();
            List<Models.Model_HistorySign> retList = new List<Models.Model_HistorySign>();
            
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutySignAttw_GetListHistorySignByLocaIDNow", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 20).Value = LocationID;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 50).Value = EmpNo;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            for(int i= 0; i < dt.Rows.Count; i ++)
            {
                Models.Model_HistorySign model = new Models.Model_HistorySign();
                model.LocationDetailName = dt.Rows[i]["LocationDetailName"].ToString();
                model.Status = Convert.ToInt32(dt.Rows[i]["Status"].ToString());
                model.Memo = dt.Rows[i]["Memo"].ToString();
                model.Workdate = dt.Rows[i]["WorkDate"].ToString();
                model.Image = dt.Rows[i]["Img"].ToString();
                model.X = Convert.ToInt32(dt.Rows[i]["X"].ToString());
                model.Y = Convert.ToInt32(dt.Rows[i]["Y"].ToString());
                retList.Add(model);
            }    
            return retList;
        }

        public List<Models.Model_HistorySign> getHistorySignByOndutyAttwID(int OndutyAttwID)
        {
            DataTable dt = new DataTable();
            List<Models.Model_HistorySign> retList = new List<Models.Model_HistorySign>();

            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutySignAttw_GetListHistorySignByOnAttwID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OndutyAttwSignID", SqlDbType.Int).Value = OndutyAttwID;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Models.Model_HistorySign model = new Models.Model_HistorySign();
                model.LocationDetailName = dt.Rows[i]["LocationDetailName"].ToString();
                model.Status = Convert.ToInt32(dt.Rows[i]["Status"].ToString());
                model.Memo = dt.Rows[i]["Memo"].ToString();
                model.Workdate = dt.Rows[i]["WorkDate"].ToString();
                model.Image = dt.Rows[i]["Img"].ToString();
                model.X = Convert.ToInt32(dt.Rows[i]["X"].ToString());
                model.Y = Convert.ToInt32(dt.Rows[i]["Y"].ToString());
                retList.Add(model);
            }
            return retList;
        }

        public DataTable getHistorySignByLocationIDNowDt(string LocationID, string EmpNo)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutySignAttw_GetListHistorySignByLocaIDNow", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LocationID", SqlDbType.VarChar, 20).Value = LocationID;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 50).Value = EmpNo;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            
            return dt;
        }

        public DataTable getHistorySignByOndutyAttwIDDt(int OndutyAttwID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("OndutySignAttw_GetListHistorySignByOnAttwID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OndutyAttwSignID", SqlDbType.Int).Value = OndutyAttwID;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }

            return dt;
        }

        public string Testconnect()
        {
            string retval = "";
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("Test_PrintOke", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Para1", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@out", SqlDbType.VarChar,50).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                retval = cmd.Parameters["@out"].Value.ToString();
                cn.Close();
            }
                return retval;
        }
         
    }
}
