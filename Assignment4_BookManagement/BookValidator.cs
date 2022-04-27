using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4_BookManagement
{
    public class BookValidator
    {
        public const string BookIdField = "Book ID";
        public const string BookNameField = "Book Name";
        public const string BookPriceField = "Book Price";
        public string BookIdStr
        {
            get; set;
        }
        public string BookNameStr
        {
            get; set;
        }
        public string BookPriceStr
        {
            get; set;
        }
        public BookValidator()
        {
        }
        public bool ValidateBookId(string BookIdStr, out string FieldMsg)
        {
            if (String.IsNullOrWhiteSpace(BookIdStr))
            {
                FieldMsg = "The Book ID is required not blank.";
                return false;
            }
            try
            {
                int bookId = int.Parse(BookIdStr);
                if (bookId < 0)
                {
                    FieldMsg = "Book ID must not be a negative integer. ";
                    return false;
                }
                else
                {
                    FieldMsg = String.Empty;
                    return true;
                }
            }
            catch (Exception)
            {
                FieldMsg = "Book ID must be an integer. ";
                return false;
            }
        }
        public bool ValidateBookName(string BookNameStr, out string FieldMsg) 
        {
            if (String.IsNullOrWhiteSpace(BookNameStr))
            {
                FieldMsg = "The Book Name is required not blank.";
                return false;
            }
            else
            {
                FieldMsg = String.Empty;
                return true;
            }
        }
        public bool ValidateBookPrice(string BookPriceStr, out string FieldMsg)
        {
            if (String.IsNullOrWhiteSpace(BookPriceStr))
            {
                FieldMsg = "The Book Price is required not blank.";
                return false;
            }
            try
            {
                float bookPrice = float.Parse(BookPriceStr);
                if (bookPrice < 0)
                {
                    FieldMsg = "Book Price must not be a negative float. ";
                    return false;
                }
                else
                {
                    FieldMsg = String.Empty;
                    return true;
                }
            }
            catch (Exception)
            {
                FieldMsg = "Book Price must be a float. ";
                return false;
            }
        }
        public Dictionary<string, string> ValidateBook()
        {
            string IdMsg;
            string NameMsg;
            string PriceMsg;

            ValidateBookId(BookIdStr, out IdMsg);
            ValidateBookName(BookNameStr, out NameMsg);
            ValidateBookPrice(BookPriceStr, out PriceMsg);

            bool IsValid = true;

            IsValid = IsValid && IdMsg == String.Empty;
            IsValid = IsValid && NameMsg == String.Empty;
            IsValid = IsValid && PriceMsg == String.Empty;

            Dictionary<string, string> FieldsMsg = new Dictionary<string, string>();
            if (!IsValid)
            {
                FieldsMsg.Add(BookIdField, IdMsg);
                FieldsMsg.Add(BookNameField, NameMsg);
                FieldsMsg.Add(BookPriceField, PriceMsg);
            }

            return FieldsMsg;
        }
        public Book GetBook()
        {
            return new Book()
            {
                BookId = int.Parse(this.BookIdStr.Trim()),
                BookName = this.BookNameStr.Trim(),
                BookPrice = float.Parse(this.BookPriceStr.Trim())
            };
        }
    }
}
