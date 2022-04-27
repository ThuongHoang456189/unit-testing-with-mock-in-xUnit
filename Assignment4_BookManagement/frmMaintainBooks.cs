using DataAccess;
using DataAccess.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment4_BookManagement
{
    
    public partial class frmMaintainBooks : Form
    {
        private DataTable BookTable;
        private BookData BookData;
        private BookValidator bookValidator;
        //private IBookDataAccess _bookDataAccess;
        public frmMaintainBooks()
        {
            InitializeComponent();
            BookTable = new DataTable();
            BookData = new BookData(BookTable);
            bookValidator = new BookValidator();
            txtBookID.Enabled = false;
        }

        public frmMaintainBooks(IBookDataAccess bookDataAccess) : this()
        {
            //this._bookDataAccess = bookDataAccess;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            new frmBookReport(BookTable).ShowDialog();
        }
        private void SetUpDataGridView()
        {
            BookTable = BookData.LoadBooks();

            //BookTable = _bookDataAccess.LoadBooks();

            BookTable.PrimaryKey = new DataColumn[] { BookTable.Columns["BookID"] };

            bsBooks.DataSource = BookTable;

            dgvBookList.DataSource = bsBooks;

            bnBookList.BindingSource = bsBooks;
        }
        private void ResetTextFields()
        {
            txtBookID.DataBindings.Clear();
            txtBookName.DataBindings.Clear();
            txtBookPrice.DataBindings.Clear();

            txtBookID.DataBindings.Add("Text", bsBooks, "BookID");
            txtBookName.DataBindings.Add("Text", bsBooks, "BookName");
            txtBookPrice.DataBindings.Add("Text", bsBooks, "BookPrice");
        }
        private void frmMaintainBooks_Load(object sender, EventArgs e)
        {
            SetUpDataGridView();

            ResetTextFields();

            ShowSumOfPrice();
        }
        private void ShowSumOfPrice()
        {
            DataView dataView = BookTable.DefaultView;
            string filter = "BookName like '%" + txtFilter.Text + "%'";
            dataView.RowFilter = filter;
            lblPriceSum.Text = "Sum of Prices: " + BookTable.Compute("SUM(BookPrice)", filter);
        }
        private string GetFieldMsg(string FieldName, Dictionary<string, string> FieldsMsg)
        {
            foreach(var pair in FieldsMsg)
            {
                if (pair.Key == FieldName)
                    return (pair.Value);
            }
            return "a";
        }
        private void ResetFeedBackMsg()
        {
            lblBookIdMsg.Text = String.Empty;
            lblBookNameMsg.Text = String.Empty;
            lblBookPriceMsg.Text = String.Empty;
            lblActionFeedback.Text = String.Empty;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetFeedBackMsg();
            try
            {
                string IdStr = txtBookID.Text;
                string NameStr = txtBookName.Text;
                string PriceStr = txtBookPrice.Text;
                bookValidator.BookIdStr =  String.IsNullOrEmpty(IdStr) ? "0" : IdStr;
                bookValidator.BookNameStr = NameStr;
                bookValidator.BookPriceStr = PriceStr;
                Dictionary<string, string> FieldsMsg = bookValidator.ValidateBook();
                if (FieldsMsg.Keys.Count == 0)
                {
                    string feedback;
                    BookData.InsertBook(bookValidator.GetBook(), out feedback);
                    //_bookDataAccess.InsertBook(bookValidator.GetBook(), out feedback);
                    lblActionFeedback.Text = feedback;
                }
                else
                {
                    lblBookIdMsg.Text = GetFieldMsg(BookValidator.BookIdField, FieldsMsg);
                    lblBookNameMsg.Text = GetFieldMsg(BookValidator.BookNameField, FieldsMsg);
                    lblBookPriceMsg.Text = GetFieldMsg(BookValidator.BookPriceField, FieldsMsg);
                }
                
            }
            catch (Exception ex)
            {
                lblActionFeedback.Text = ex.Message;
            }
            SetUpDataGridView();
            ShowSumOfPrice();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ResetFeedBackMsg();
            try
            {
                string IdStr = txtBookID.Text;
                string NameStr = txtBookName.Text;
                string PriceStr = txtBookPrice.Text;
                bookValidator.BookIdStr = IdStr;
                bookValidator.BookNameStr = NameStr;
                bookValidator.BookPriceStr = PriceStr;
                Dictionary<string, string> FieldsMsg = bookValidator.ValidateBook();
                if (FieldsMsg.Keys.Count == 0)
                {
                    string feedback;
                    BookData.UpdateBook(bookValidator.GetBook(), out feedback);
                    lblActionFeedback.Text = feedback;
                }
                else
                {
                    lblBookIdMsg.Text = GetFieldMsg(BookValidator.BookIdField, FieldsMsg);
                    lblBookNameMsg.Text = GetFieldMsg(BookValidator.BookNameField, FieldsMsg);
                    lblBookPriceMsg.Text = GetFieldMsg(BookValidator.BookPriceField, FieldsMsg);
                }
            }
            catch (Exception ex)
            {
                lblActionFeedback.Text = ex.Message;
            }
            SetUpDataGridView();
            ShowSumOfPrice();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ResetFeedBackMsg();
            try
            {
                bookValidator.BookIdStr = txtBookID.Text;
                string feedback;
                if (bookValidator.ValidateBookId(txtBookID.Text, out feedback))
                {
                    // Khuc nay can confirm
                    BookData.DeleteBook(int.Parse(bookValidator.BookIdStr), out feedback);
                } 
                lblActionFeedback.Text = feedback;
            }
            catch (Exception ex)
            {
                lblActionFeedback.Text = ex.Message;
            }
            SetUpDataGridView();
            ShowSumOfPrice();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ShowSumOfPrice();
        }
    }
}
