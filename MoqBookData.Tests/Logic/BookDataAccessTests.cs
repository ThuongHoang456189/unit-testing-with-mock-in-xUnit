using Autofac.Extras.Moq;
using DataAccess;
using DataAccess.Utilities;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAccess.Logic;
using Moq;
using System.Reflection;

namespace MoqBookData.Tests.Logic
{
    public class BookDataAccessTests
    {
        [Fact]
        public void ExampleTest()
        {
            int a = 0;
            Assert.Equal(0, a);
        }

        [Theory]
        [InlineData("Book1", "12.2")]
        [InlineData("Book1", "10")]
        public void CreateBook_Successfully(string bookName, string bookPrice)
        {
            BookDataAccess bookDataAccess = new BookDataAccess(null, null);
            Book expected = new Book
            {
                BookName = bookName,
                BookPrice = float.Parse(bookPrice)
            };

            var actual = bookDataAccess.CreateBook(bookName, bookPrice);

            Assert.Equal(expected.BookName, actual.BookName);
            Assert.Equal(expected.BookPrice, actual.BookPrice);
        }
        [Theory]
        [InlineData("", "-12.2", "bookName")]
        [InlineData("", "a", "bookName")]
        [InlineData(null, "a", "bookName")]
        [InlineData("", null, "bookName")]
        [InlineData(null, null, "bookName")]
        [InlineData("Book1", "-12.2", "bookPrice")]
        [InlineData("Book2", "12.2a", "bookPrice")]
        [InlineData("Book3", "a", "bookPrice")]
        public void CreateBook_ThrowsException(string bookName, string bookPrice, string expectedInvalidParameter)
        {
            BookDataAccess bookDataAccess = new BookDataAccess(null, null);

            var ex = Record.Exception(() => bookDataAccess.CreateBook(bookName, bookPrice));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            if(ex is ArgumentException argEx)
            {
                Assert.Equal(expectedInvalidParameter, argEx.ParamName);
            }
        }
        private DataTable GetSampleBookTable()
        {
            List<Book> books = new List<Book>
            {
                new Book
                {
                    BookName = "Book1",
                    BookPrice = 10.3f
                },
                new Book
                {
                    BookName = "Book2",
                    BookPrice = 9.3f
                },
                new Book
                {
                    BookName = "Book3",
                    BookPrice = 11.3f
                }
            };

            return ToDataTable<Book>(books);
        }
        [Fact]
        public void LoadBooks_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                 string sql = "select BookID, BookName, BookPrice from Books";

                DataTable dataTable = new DataTable();

                mock.Mock<ISqlServerDataAccess>()
                    .Setup(x => x.LoadData(sql))
                    .Returns(GetSampleBookTable());

                var dataAccess = mock.Create<BookDataAccess>();

                var expected = GetSampleBookTable();

                var actual = dataAccess.LoadBooks();

                Assert.True(actual != null);
                Assert.Equal(expected.Rows.Count, actual.Rows.Count);
                for (int i = 0; i < expected.Rows.Count; i++)
                {
                    object[] expectedValues = expected.Rows[i].ItemArray;
                    object[] actualValues = actual.Rows[i].ItemArray;
                    Assert.Equal(expectedValues[1], actualValues[1]);
                    Assert.Equal(expectedValues[2], actualValues[2]);
                }
            }
        }
        [Fact]
        public void InsertBook_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var book = GetSampleBooks()[0];
                
                string sql = $"insert Books(BookName, BookPrice) values ('{book.BookName}', {book.BookPrice})";
                string ResultMsg;

                DataTable dataTable = new DataTable();

                mock.Mock<ISqlServerDataAccess>()
                    .Setup(x => x.SaveData(sql));

                var dataAccess = mock.Create<BookDataAccess>();

                dataAccess.InsertBook(book, out ResultMsg);

                mock.Mock<ISqlServerDataAccess>()
                    .Verify(x => x.SaveData(sql), Times.Exactly(1));
            }
        }
        private List<Book> GetSampleBooks()
        {
            List<Book> books = new List<Book>
            {
                new Book
                {
                    BookName = "Book1",
                    BookPrice = 10.3f
                },
                new Book
                {
                    BookName = "Book2",
                    BookPrice = 9.3f
                },
                new Book
                {
                    BookName = "Book3",
                    BookPrice = 11.3f
                }
            };

            return books;
        }
        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable();
            // Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // Inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }
            // Put a breakpoint here and check datatable
            return dataTable;
        }
        
    }
}
