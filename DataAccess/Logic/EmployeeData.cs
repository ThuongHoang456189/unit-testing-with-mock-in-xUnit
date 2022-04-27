using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class EmployeeData
    {
        private static readonly string ConnectionString;
        static EmployeeData()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["BookStoreDB"].ConnectionString;
        }
        public Employee Login(string EmpID)
        {
            Employee employee = null;
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                
                string SQL = "select EmpID, EmpPassword, EmpRole from Employee where EmpID = @id";
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@id", EmpID);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    employee = new Employee { EmpID = sqlDataReader.GetString(0), EmpPassword = sqlDataReader.GetString(1), EmpRole = sqlDataReader.GetBoolean(2) };
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }   
            return employee;
        }
        public bool ChangePassword(string EmpID, string NewPwd)
        {
            bool isOk = false;
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                string SQL = "update Employee set EmpPassword = @pwd where EmpID = @id";
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@id", EmpID);
                cmd.Parameters.AddWithValue("@pwd", NewPwd);
                sqlConnection.Open();
                if(cmd.ExecuteNonQuery() > 0)
                    isOk = true;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return isOk;
        }
    }
}
