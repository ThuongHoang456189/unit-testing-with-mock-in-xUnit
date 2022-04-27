using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class SqlServerDataAccess : ISqlServerDataAccess
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["BookStoreDB"].ConnectionString;
        public SqlServerDataAccess(string id = "Default")
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        public DataTable LoadData(string sql)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            return dataTable;
        }

        public bool IsDataExisting(string sql)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = command.ExecuteReader().HasRows;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            return result;
        }

        public bool SaveData(string sql)
        {
            bool result = false;
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = command.ExecuteNonQuery() > 0;
                }
                catch(SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            return result;
        }
    }
}
