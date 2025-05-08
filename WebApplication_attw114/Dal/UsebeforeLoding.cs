using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication_attw114.Dal
{
    public class UsebeforeLoding
    {
        public const string ConnecStr = "Data Source=10.224.24.30,4433;Initial Catalog=CertificateDB;User ID=formsign;Password=1234567Aa";

        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 12000;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 檢查未分配值的輸出參數,將其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 12000;
            return command;
        }

        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(ConnecStr);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            connection.Close();
            return returnReader;

        }
    }
}
