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

namespace DataAccess
{
    public class BookDataAccess : IBookDataAccess
    {
        private DataTable _bookTable;
        private ISqlServerDataAccess _database;
        public BookDataAccess(DataTable bookTable, ISqlServerDataAccess database)
        {
            this._bookTable = bookTable;
            this._database = database;
        }
        private bool ValidateBookName(string bookNameStr)
        {
            if (String.IsNullOrWhiteSpace(bookNameStr))
            { 
                return false;
            }
            else
            {
                
                return true;
            }
        }
        private bool ValidateBookPrice(string bookPriceStr)
        {
            if (String.IsNullOrWhiteSpace(bookPriceStr))
            {
                return false;
            }
            try
            {
                float bookPrice = float.Parse(bookPriceStr);
                if (bookPrice < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Book CreateBook(string bookName, string bookPrice)
        {
            Book book = new Book();

            if (ValidateBookName(bookName))
            {
                book.BookName = bookName;
            }
            else
            {
                throw new ArgumentException("The value was not valid", "bookName");
            }

            if (ValidateBookPrice(bookPrice))
            {
                book.BookPrice = float.Parse(bookPrice);
                // book.BookPrice = float.Parse(bookPrice) + 1;
            }
            else
            {
                throw new ArgumentException("The value was not valid", "bookPrice");
            }

            return book;
        }
        public DataTable LoadBooks()
        {
            string sql = "select BookID, BookName, BookPrice from Books";
            try
            {
                _bookTable = _database.LoadData(sql);
            }catch (Exception ex)
            {
                throw ex;
            }
            return _bookTable;
        }
        public bool IsBookIdExisting(int BookId) 
        {
            string sql = "select BookID from Books where BookID = @ID";

            sql = sql.Replace("@ID", $"{BookId}");

            bool existed;
            try
            {
                existed = _database.IsDataExisting(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return existed;
        }
        public void InsertBook(Book Book, out string ResultMsg)
        {
            string sql = "insert Books(BookName, BookPrice) values (@Name, @Price)";

            sql = sql.Replace("@Name", $"'{Book.BookName}'");
            sql = sql.Replace("@Price", $"{Book.BookPrice}");

            try
            {
                bool isOk = _database.SaveData(sql);
                ResultMsg = isOk ? "Inserted successfully " : $"Something wrong happened when insert new Book with ID {Book.BookId}!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
