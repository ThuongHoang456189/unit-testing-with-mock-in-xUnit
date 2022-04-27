using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Logic
{
    public interface IBookDataAccess
    {
        Book CreateBook(string bookName, string bookPrice);
        DataTable LoadBooks();
        void InsertBook(Book Book, out string ResultMsg);

        bool IsBookIdExisting(int BookId);
    }
}
