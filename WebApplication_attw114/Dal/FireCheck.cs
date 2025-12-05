using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApplication_attw114.Models;
using WebApplication_attw114.Models.PatrolFit.Response;

namespace WebApplication_attw114.Dal
{
    public class FireCheck
    {
        public FireCheck() { }
        string strconn = "Data Source=10.224.69.61,8989;Initial Catalog=FireCheck;User ID=formsign;Password=!System114&";
        /// <summary>
        /// Get list of rules by device code
        /// </summary>
        /// <param name="code_device"></param>
        /// <returns></returns>
        public List<Models.RuleInfor> GetlistRule(string code_device)
        {
            var result = new List<Models.RuleInfor>();
            using (SqlConnection cn = new SqlConnection(strconn))
            using (SqlCommand cmd = new SqlCommand("UP_Devices_GetRuleByCode", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", code_device ?? string.Empty);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Models.RuleInfor
                        {
                            Id = DbHelperSQL.SafeGet<int>(reader, "ID"),
                            RuleName = DbHelperSQL.SafeGet<string>(reader, "RuleName")
                        };
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// get list of rules by device code
        /// </summary>
        /// <param name="code_device"></param>
        /// <returns></returns>
        public Devices GetDevices(string code_device) {
            var result = new Devices();
            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("UP_Devices_GetInforByCode", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", code_device ?? string.Empty);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.Code = DbHelperSQL.SafeGet<string>(reader, "Code");
                        result.FactoryName = DbHelperSQL.SafeGet<string>(reader, "FactoryName");
                        result.ZoneName = DbHelperSQL.SafeGet<string>(reader, "ZoneName");
                        result.Id = DbHelperSQL.SafeGet<int>(reader, "Id");
                        result.TypeName = DbHelperSQL.SafeGet<string>(reader, "TypeName");
                        result.Location = DbHelperSQL.SafeGet<string>(reader, "Location");
                        result.Type = DbHelperSQL.SafeGet<int>(reader, "Type");
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// add checked rule
        /// </summary>
        /// <param name="checkedDTO"></param>
        public void Add_CheckedRule(Models.RuleCheckedList checkedDTO)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("Up_RuleCheckedList_Sign", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdChecked", SqlDbType.Int).Value = checkedDTO.IdChecked;
                cmd.Parameters.Add("@RuleID", SqlDbType.Int).Value = checkedDTO.RuleID;
                cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 300).Value = checkedDTO.Memo;
                cmd.Parameters.Add("@IsOk", SqlDbType.Bit).Value = checkedDTO.IsOk;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        /// <summary>
        /// sign check list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int FireCheckSign(Models.CheckedList model)
        {
            int Retval = 0;
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("Up_CheckedList_Sign", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 200).Value = model.Memo;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                cmd.Parameters.Add("@DeviceID", SqlDbType.Int).Value = model.DeviceID;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                Retval = Convert.ToInt32(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return Retval;
        }
        public FcUser GetUser(int iduser)
        {
            var result = new FcUser();
            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("UP_FcUser_GetInfor", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = iduser;
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.EmpNo = DbHelperSQL.SafeGet<string>(reader, "EmpNo");
                        result.EmpName = DbHelperSQL.SafeGet<string>(reader, "EmpName");
                        result.Id = DbHelperSQL.SafeGet<int>(reader, "Id");
                    }
                }
            }
            return result;
        }
    }
}
