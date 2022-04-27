using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DataAccess.Logic;
using DataAccess.Utilities;
using System.Data.Common;

namespace DataAccess
{
    public class BookData
    {
        private static readonly string ConnectionString;
        private SqlDataAdapter adapter;
        private DataTable BookTable;
        public BookData(DataTable BookTable)
        {
            this.BookTable = BookTable;
        }
        static BookData()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["BookStoreDB"].ConnectionString;
        }
        public DataTable LoadBooks()
        {
            string SQL = "select BookID, BookName, BookPrice, Quantity from Books";
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                adapter = new SqlDataAdapter(cmd);
                BookTable.Clear();
                adapter.Fill(BookTable);
            }catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return BookTable;
        }
        public bool IsBookIdExisting(int BookId) 
        {
            string SQL = "select BookID from Books where BookID = @ID";
            SqlConnection sqlConnection = null;
            bool existed;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@ID", BookId);
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    existed = true;
                else
                    existed = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return existed;
        }
        public void InsertBook(Book Book, out string ResultMsg)
        {
            //if (IsBookIdExisting(Book.BookId))
            //{
            //    ResultMsg = $"Book ID {Book.BookId} existed!";
            //    return;
            //}
            string SQL = "insert Books(BookName, BookPrice, Quantity) values (@Name, @Price, @Amount)";
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@Name", Book.BookName);
                cmd.Parameters.AddWithValue("@Price", Book.BookPrice);
                cmd.Parameters.AddWithValue("@Amount", Book.Quantity);
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                bool isOk = cmd.ExecuteNonQuery() > 0;
                ResultMsg = isOk ? "Inserted successfully ": $"Something wrong happened when insert a new book!";
            }
            catch (Exception ex)
            {
                ResultMsg = $"Something wrong happened when insert new Book with ID {Book.BookId}!";
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

        }
        public void UpdateBook(Book Book, out string ResultMsg)
        {
            if (!IsBookIdExisting(Book.BookId))
            {
                ResultMsg = $"Book ID {Book.BookId} does not exist!";
                return;
            }
            string SQL = "update Books set BookName = @Name, BookPrice = @Price, Quantity = @Amount where BookID = @ID";
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@ID", Book.BookId);
                cmd.Parameters.AddWithValue("@Name", Book.BookName);
                cmd.Parameters.AddWithValue("@Price", Book.BookPrice);
                cmd.Parameters.AddWithValue("@Amount", Book.Quantity);
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                bool isOk = cmd.ExecuteNonQuery() > 0;
                ResultMsg = isOk ? $"Updated Book with ID {Book.BookId} successfully" : $"Something wrong happened when update Book with ID {Book.BookId}!";
            }
            catch (Exception ex)
            {
                ResultMsg = $"Something wrong happened when update Book with ID {Book.BookId}!";
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        public void DeleteBook(int BookID, out string ResultMsg)
        {
            if (!IsBookIdExisting(BookID))
            {
                ResultMsg = $"Book ID {BookID} does not exist!";
                return;
            }
            string SQL = "delete from Books where BookID = @ID";
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(SQL, sqlConnection);
                cmd.Parameters.AddWithValue("@ID", BookID);
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                bool isOk = cmd.ExecuteNonQuery() > 0;
                ResultMsg = isOk ? $"Deleted Book with ID { BookID } successfully" : $"Something wrong happened when delete Book with ID { BookID }!";
            }
            catch (Exception ex)
            {
                ResultMsg = $"Something wrong happened when delete Book with ID {BookID}!";
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
    }
}
