using Assignment4_BookManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace temp
{
    public class Program
    {
        static void Main(string[] args)
        {
            BookValidator bookValidator = new BookValidator();
            bookValidator.BookIdStr = "1";
            bookValidator.BookNameStr = "a";
            bookValidator.BookPriceStr = "1";
            Console.WriteLine(bookValidator.ValidateBook());
        }
    }
}
