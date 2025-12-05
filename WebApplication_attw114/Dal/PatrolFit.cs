using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using WebApplication_attw114.Models.PatrolFit.Response;
using WebApplication_attw114.Models;
using System.Linq;
using WebApplication_attw114.Models.PatrolFit.DTO;

namespace WebApplication_attw114.Dal
{
    public class PatrolFit
    {
        string strconn = "Data Source=10.224.69.61,8989;Initial Catalog=CertificateDB30;User ID=formsign;Password=!System114&";
        /// <summary>
        /// Lấy Tiêu Chuẩn Của Điểm Kiểm tra
        /// </summary>
        /// <param name="code_point"></param>
        /// <param name="typepatrol"></param>
        /// <returns></returns>
        public DataTable GetlistRule(string code_point, int typepatrol)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolPoint_GetRuleByCodePoint", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Code_point", SqlDbType.VarChar, 20).Value = code_point;
                cmd.Parameters.Add("@TypePatrol", SqlDbType.Int).Value = typepatrol;
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Lấy danh sách các tiêu chuẩn của điểm kiểm tra theo mã điểm và loại tuần tra
        /// </summary>
        /// <param name="code_point"></param>
        /// <param name="typepatrol"></param>
        /// <returns></returns>
        public List<Models.PatrolFit.Response.RuleInfor> GetRuleInfors(string code_point, int typepatrol)
        {
            var result = new List<Models.PatrolFit.Response.RuleInfor>();
            using (SqlConnection cn = new SqlConnection(strconn))
            using (SqlCommand cmd = new SqlCommand("UP_PatrolPoint_GetRuleByCodePoint", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code_point", code_point ?? string.Empty);
                cmd.Parameters.AddWithValue("@TypePatrol", typepatrol);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Models.PatrolFit.Response.RuleInfor
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
        /// Lay danh sach cac diem kiem tra
        /// </summary>
        /// <param name="nowDTO"></param>
        /// <returns></returns>
        public DataSet PatrolPoint_GetHistoryByCodeNow(Models.PatrolFit.DTO.PatrolGetNowDTO nowDTO)
        {
            DataSet dt = new DataSet();
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolPoint_GetHistoryByCodeNow", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Code_point", SqlDbType.VarChar, 20).Value = nowDTO.Code_point;
                cmd.Parameters.Add("@TypePatrol", SqlDbType.Int).Value = nowDTO.TypePatrol;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = nowDTO.EmpNo;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                cn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Lấy danh sách các điểm kiểm tra theo mã điểm và loại tuần tra
        /// </summary>
        /// <param name="nowDTO"></param>
        /// <returns></returns>
        public HistorySignNowList PatrolPoint_GetHistory(Models.PatrolFit.DTO.PatrolGetNowDTO nowDTO)
        {
            var result = new HistorySignNowList
            {
                List = new List<HistorySignNow>(),
                Total = 0
            };

            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("UP_PatrolPoint_GetHistoryByCodeNow", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code_point", nowDTO.Code_point ?? string.Empty);
                cmd.Parameters.AddWithValue("@TypePatrol", nowDTO.TypePatrol);
                cmd.Parameters.AddWithValue("@EmpNo", nowDTO.EmpNo ?? string.Empty);

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var workDate = DbHelperSQL.SafeGet<DateTime?>(reader, "WorkDate");

                        var item = new HistorySignNow
                        {
                            Id = DbHelperSQL.SafeGet<int>(reader, "ID"),
                            CodePoint = DbHelperSQL.SafeGet<string>(reader, "LocationID"),
                            NamePoint = DbHelperSQL.SafeGet<string>(reader, "LocationDetailName"),
                            StatusSign = DbHelperSQL.SafeGet<string>(reader, "status_sign"),
                            StatusPlace = DbHelperSQL.SafeGet<string>(reader, "status_place")??"",
                            Memo = DbHelperSQL.SafeGet<string>(reader, "Memo")??"",
                            WorkDate = workDate.HasValue ? workDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                            X = DbHelperSQL.SafeGet<int>(reader, "X"),
                            Y = DbHelperSQL.SafeGet<int>(reader, "Y")
                        };
                        if (item.StatusSign == "da_ky")
                        {
                            item.StatusSign = "Đã Ký";
                        }
                        else
                        {
                            item.StatusSign = "Chưa Ký";
                        }
                        if (item.StatusPlace != "")
                        {
                            switch (item.StatusPlace)
                            {
                                case "binh_thuong":
                                    item.StatusPlace = "Bình Thường";
                                    break;
                                case "bat_thuong":
                                    item.StatusPlace = "Bất Thường";
                                    break;
                            }
                        }
                        result.List.Add(item);
                    }
                }
            }

            result.Total = result.List.Count;
            return result;
        }
        /// <summary>
        /// Lấy thông tin điểm kiểm tra theo mã điểm
        /// </summary>
        /// <param name="codePoint"></param>
        /// <returns></returns>
        public PointInfor GetPointInfor(string codePoint)
        {
            var result = new PointInfor();
            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("UP_PatrolPoint_GetInforByCode", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodePoint", codePoint ?? string.Empty);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.Name = DbHelperSQL.SafeGet<string>(reader, "Name");
                        result.CodePoint = DbHelperSQL.SafeGet<string>(reader, "Code_Point");
                        result.FullName = DbHelperSQL.SafeGet<string>(reader, "FullName");
                        result.AreaID = DbHelperSQL.SafeGet<int>(reader, "AreaID");
                        result.UrlImage = DbHelperSQL.SafeGet<string>(reader, "UrlImage");
                        result.Lati = DbHelperSQL.SafeGet<float>(reader, "Latitude");
                        result.Longti = DbHelperSQL.SafeGet<float>(reader, "Longtitude");
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// get infor by userID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public PatrolSignInforDTO GetInforByUserID(string userid)
        {
            var result = new PatrolSignInforDTO();
            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("Up_PatrolSign_GetinforByUserID", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userid ?? string.Empty);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.EmpNo = DbHelperSQL.SafeGet<string>(reader, "EmpNo");
                        result.EmpName = DbHelperSQL.SafeGet<string>(reader, "EmpName");
                        result.UserID = userid;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Thêm kết quả kiểm tra cac tieu chuan
        /// </summary>
        /// <param name="model"></param>
        public void Add_CheckedRule(Models.PatrolFit.DTO.PatrolCheckedDTO checkedDTO)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolPointCheckResult_Add", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PointCheckedID", SqlDbType.Int).Value = checkedDTO.PointCheckedID;
                cmd.Parameters.Add("@RuleID", SqlDbType.Int).Value = checkedDTO.RuleID;
                cmd.Parameters.Add("@ImageName", SqlDbType.VarChar, 100).Value = checkedDTO.ImageName;
                cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 300).Value = checkedDTO.Memo;
                cmd.Parameters.Add("@IsOk", SqlDbType.Bit).Value = checkedDTO.IsOk;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public void Update_PatrolPointlatilongti(Models.PatrolFit.DTO.PointUpdateDTO updateDTO)
        {
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolPoint_UpdateLatilongti", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodePoint", SqlDbType.VarChar, 20).Value = updateDTO.CodePoint;
                cmd.Parameters.Add("@Lati", SqlDbType.Float).Value = updateDTO.Lati;
                cmd.Parameters.Add("@Longti", SqlDbType.Float).Value = updateDTO.Longti;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        /// <summary>
        /// Ký Tuần Tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int PatrolSignOld(Models.PatrolFit.DTO.PatrolSignDTO model)
        {
            int Retval = 0;
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolRecord_Sign", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Code_Point", SqlDbType.VarChar, 20).Value = model.code_Point;
                cmd.Parameters.Add("@EmpNo", SqlDbType.VarChar, 20).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 50).Value = model.EmpName;
                cmd.Parameters.Add("@TypePatrol", SqlDbType.Int).Value = model.TypePatrol;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                Retval = Convert.ToInt32(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return Retval;
        }
        /// <summary>
        /// Ký Tuần Tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int PatrolSign(Models.PatrolFit.DTO.PatrolSignDTO model)
        {
            int Retval = 0;
            using (SqlConnection cn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand("UP_PatrolRecord_SignUD", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Code_Point", SqlDbType.VarChar, 20).Value = model.code_Point;
                cmd.Parameters.Add("@EmpNo", SqlDbType.NVarChar, 50).Value = model.EmpNo;
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar, 50).Value = model.EmpName;
                cmd.Parameters.Add("@TypePatrol", SqlDbType.Int).Value = model.TypePatrol;
                cmd.Parameters.Add("@Lati", SqlDbType.Float).Value = model.Lati;
                cmd.Parameters.Add("@Longti", SqlDbType.Float).Value = model.Longti;
                cmd.Parameters.Add("@Reval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                Retval = Convert.ToInt32(cmd.Parameters["@Reval"].Value.ToString());
                cn.Close();
            }
            return Retval;
        }
        /// <summary>
        /// Lấy thông tin tuần tra theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RecordInfor GetRecordInforById(int id)
        {
            RecordInfor info = null;
            using (SqlConnection cn = new SqlConnection(strconn))
            using (SqlCommand cmd = new SqlCommand("Up_PatrolRecord_GetInforByID", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                cn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        info = new RecordInfor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmpNo = reader["EmpNo"]?.ToString(),
                            EmpName = reader["EmpName"]?.ToString(),
                            AreaName = reader["AreaName"]?.ToString(),
                            AreaID= reader.GetInt32(reader.GetOrdinal("AreaID")),
                            FrameName = reader["FrameName"]?.ToString(),
                            UrlImage = reader["UrlImage"]?.ToString(),
                            TypePatrol = Convert.ToInt32(reader["TypePatrol"]),
                            DatePatrol = reader["DatePatrol"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DatePatrol"])
                                : DateTime.MinValue
                        };
                    }
                }
            }
            return info;
        }
        /// <summary>
        /// Lấy danh sách các tiêu chuẩn đã kiểm tra của điểm kiểm tra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Models.PatrolFit.Response.HistorySignID> PatrolRecord_GetListChecked(int id)
        {
            var result = new List<Models.PatrolFit.Response.HistorySignID>();

            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("Up_PatrolRecord_GetListCheckedRule", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID",id);

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var workDate = DbHelperSQL.SafeGet<DateTime?>(reader, "WorkDate");

                        var item = new Models.PatrolFit.Response.HistorySignID
                        {
                            Id = DbHelperSQL.SafeGet<int>(reader, "ID"),
                            PointID = DbHelperSQL.SafeGet<int>(reader, "PointID"),
                            CodePoint = DbHelperSQL.SafeGet<string>(reader, "LocationID"),
                            NamePoint = DbHelperSQL.SafeGet<string>(reader, "LocationDetailName"),
                            StatusSign = DbHelperSQL.SafeGet<string>(reader, "status_sign"),
                            StatusPlace = DbHelperSQL.SafeGet<string>(reader, "status_place") ?? "",
                            PointCheckedID = DbHelperSQL.SafeGet<int>(reader, "PointCheckedID"),
                            WorkDate = workDate.HasValue ? workDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                            X = DbHelperSQL.SafeGet<int>(reader, "X"),
                            Y = DbHelperSQL.SafeGet<int>(reader, "Y")
                        };
                        if (item.StatusPlace != "")
                        {
                            switch (item.StatusPlace)
                            {
                                case "binh_thuong":
                                    item.StatusPlace = "Bình Thường";
                                    break;
                                case "bat_thuong":
                                    item.StatusPlace = "Bất Thường";
                                    break;
                            }
                        }
                        result.Add(item);
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin chi tiết của điểm kiểm tra theo ID bản ghi và ID điểm
        /// </summary>
        /// <param name="idrecord"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public PointDetailViewModel GetPointDetailViewModel(int idrecord, int pointid)
        {
            // Lấy danh sách tiêu chuẩn đã kiểm tra trước
            var listPointDetail = PatrolPointCheckedResult_GetByIDReCordAndPointID(idrecord, pointid);

            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("Up_PatrolPointChecked_GetResult", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDRecord", idrecord);
                cmd.Parameters.AddWithValue("@PointID", pointid);

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var namePoint = DbHelperSQL.SafeGet<string>(reader, "Name");
                        var workDate = DbHelperSQL.SafeGet<DateTime?>(reader, "TimeCheck");
                        var statusSign = DbHelperSQL.SafeGet<string>(reader, "StatusSign");

                        return new PointDetailViewModel
                        {
                            NamePoint = namePoint,
                            SignTime = workDate.HasValue ? workDate.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty,
                            StatusSign = statusSign,
                            ListPointDetail = listPointDetail
                        };
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Lấy danh sách các tiêu chuẩn đã kiểm tra của điểm kiểm tra theo ID điểm, ID bản ghi và loại tuần tra
        /// </summary>
        /// <param name="idcheckedpoint"></param>
        /// <param name="pointid"></param>
        /// <param name="typepatrol"></param>
        /// <returns></returns>
        public List<PointDetailRule> GetPointCheckedRule(int idcheckedpoint, int pointid, int typepatrol)
        {
            var result = new List<PointDetailRule>();

            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("Up_PatrolPointCheckedResult_GetDetail", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", idcheckedpoint);
                cmd.Parameters.AddWithValue("@PointID", pointid);
                cmd.Parameters.AddWithValue("@TypePatrol", typepatrol);

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PointDetailRule
                        {
                            RuleName = DbHelperSQL.SafeGet<string>(reader, "RuleName"),
                            IsOk = DbHelperSQL.SafeGet<bool>(reader, "IsOk"),
                            Memo = DbHelperSQL.SafeGet<string>(reader, "Memo")
                        });
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách các tiêu chuẩn đã kiểm tra của điểm kiểm tra theo ID bản ghi và ID điểm
        /// </summary>
        /// <param name="idrecord"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public List<PointDetailRule> PatrolPointCheckedResult_GetByIDReCordAndPointID(int idrecord, int pointid)
        {
            var result = new List<PointDetailRule>();

            using (var cn = new SqlConnection(strconn))
            using (var cmd = new SqlCommand("Up_PatrolPointCheckedResult_GetByIDReCordAndPointID", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDRecord", idrecord);
                cmd.Parameters.AddWithValue("@PointID", pointid);

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PointDetailRule
                        {
                            RuleName = DbHelperSQL.SafeGet<string>(reader, "RuleName"),
                            IsOk = DbHelperSQL.SafeGet<bool>(reader, "IsOk"),
                            Memo = DbHelperSQL.SafeGet<string>(reader, "Memo")
                        });
                    }
                }
            }

            return result;
        }


    }
}
