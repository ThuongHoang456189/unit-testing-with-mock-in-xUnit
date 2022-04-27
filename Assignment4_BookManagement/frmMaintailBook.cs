using DataAccess;
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
    public partial class frmMaintailBook : Form
    {
        private DataTable _BookTable;
        private BookData _BookData;
        public frmMaintailBook()
        {
            InitializeComponent();
            _BookTable = new DataTable();
            _BookData = new BookData(_BookTable);
            txtBookID.Enabled = false;

        }
        private Book GetBook()
        {
            return new Book
            {
                BookId = int.Parse(txtBookID.Text.Trim()),
                BookName = txtBookName.Text.Trim(),
                BookPrice = float.Parse(txtBookPrice.Text.Trim()),
                Quantity = int.Parse(txtQuantity.Text.Trim())
            };
        }
        private void RebindTextFields()
        {
            txtBookID.DataBindings.Add("Text", bsBooks, "BookID");
            txtBookName.DataBindings.Add("Text", bsBooks, "BookName");
            txtBookPrice.DataBindings.Add("Text", bsBooks, "BookPrice");
            txtQuantity.DataBindings.Add("Text", bsBooks, "Quantity");
        }
        private void ClearTextFieldsBinding()
        {
            txtBookID.DataBindings.Clear();
            txtBookName.DataBindings.Clear();
            txtBookPrice.DataBindings.Clear();
            txtQuantity.DataBindings.Clear();
        }
        private void SetUpDataGridView()
        {
            _BookTable = _BookData.LoadBooks();

            //BookTable = _bookDataAccess.LoadBooks();

            _BookTable.PrimaryKey = new DataColumn[] { _BookTable.Columns["BookID"] };

            bsBooks.DataSource = _BookTable;

            dgvBookList.DataSource = bsBooks;

            bnBookList.BindingSource = bsBooks;

            dgvBookList.ReadOnly = true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string feedback;
                _BookData.InsertBook(GetBook(), out feedback);
                if (feedback.StartsWith("Inserted successfully"))
                {
                    MessageBox.Show(feedback, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(feedback, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _BookTable = _BookData.LoadBooks();
            ClearTextFieldsBinding();
            RebindTextFields();
            ShowSumOfPrice();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string feedback;
                _BookData.UpdateBook(GetBook(), out feedback);
                if (feedback.StartsWith("Updated"))
                {
                    MessageBox.Show(feedback, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(feedback, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _BookTable = _BookData.LoadBooks();
            ClearTextFieldsBinding();
            RebindTextFields();
            ShowSumOfPrice();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string feedback;
                _BookData.DeleteBook(int.Parse(txtBookID.Text.Trim()), out feedback);
                if (feedback.StartsWith("Deleted"))
                {
                    MessageBox.Show(feedback, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(feedback, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _BookTable = _BookData.LoadBooks();
            ClearTextFieldsBinding();
            RebindTextFields();
            ShowSumOfPrice();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            new frmBookReport(_BookTable).ShowDialog();
        }

        private void txtBookName_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("Book Name", txtBookName.Text, ref errorMsg))
            {
                e.Cancel = true;
                txtBookName.Focus();
                errorProvider.SetError(txtBookName, errorMsg);
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtBookName, null);
            }
        }

        private void txtBookPrice_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("Book Price", txtBookPrice.Text, ref errorMsg))
            {
                e.Cancel = true;
                txtBookPrice.Focus();
                errorProvider.SetError(txtBookPrice, errorMsg);
            }
            else
            {
                if (!InputValidators.InputValidators.IsValidNonNegativeFloat("Book Price", txtBookPrice.Text, ref errorMsg))
                {
                    e.Cancel = true;
                    txtBookPrice.Focus();
                    errorProvider.SetError(txtBookPrice, errorMsg);
                }
                else
                {
                    e.Cancel = false;
                    errorProvider.SetError(txtBookPrice, null);
                }
            }
        }

        private void txtQuantity_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("Quantity", txtQuantity.Text, ref errorMsg))
            {
                e.Cancel = true;
                txtQuantity.Focus();
                errorProvider.SetError(txtQuantity, errorMsg);
            }
            else
            {
                if (!InputValidators.InputValidators.IsValidNonNegativeInteger("Quantity", txtQuantity.Text, ref errorMsg))
                {
                    e.Cancel = true;
                    txtQuantity.Focus();
                    errorProvider.SetError(txtQuantity, errorMsg);
                }
                else
                {
                    e.Cancel = false;
                    errorProvider.SetError(txtQuantity, null);
                }
            }
        }
        private void ShowSumOfPrice()
        {
            DataView dataView = _BookTable.DefaultView;
            string filter = "BookName like '%" + txtBookName.Text + "%'";
            dataView.RowFilter = filter;

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.Caption = "Subtotal";
            column.ColumnName = "Subtotal";

            // Add the column to the table.
            if(!_BookTable.Columns.Contains("Subtotal"))
                _BookTable.Columns.Add(column);

            // Add 10 rows and set values.
            DataRow row;
            for (int i = 0; i < _BookTable.Rows.Count; i++)
            {
                row = _BookTable.Rows[i];
                row["Subtotal"] = double.Parse(row["BookPrice"].ToString()) * double.Parse(row["Quantity"].ToString());

                // Be sure to add the new row to the
                // DataRowCollection.
                //_BookTable.Rows.Add(row);
            }

            lblPriceSum.Text = "Sum of Subtotals: " + _BookTable.Compute("SUM(Subtotal)", filter);
        }
        private void txtBookName_TextChanged(object sender, EventArgs e)
        {
            ShowSumOfPrice();
        }
        private void txtBookName_Enter(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();
        }
        private void txtBookName_Leave(object sender, EventArgs e)
        {
            ShowSumOfPrice();
        }

        private void frmMaintailBook_Load(object sender, EventArgs e)
        {
            SetUpDataGridView();

            ClearTextFieldsBinding();

            RebindTextFields();

            ShowSumOfPrice();

            DataRow row = _BookTable.NewRow();
            lblBookName.Text += row.RowState.ToString();
            _BookTable.Rows.Add(row);
            lblBookName.Text += row.RowState.ToString();
            _BookTable.AcceptChanges();
            lblBookName.Text += row.RowState.ToString();
            _BookTable.Rows.Remove(row);
            // lblBookName.Text += row.RowState.ToString();
        }

        private void txtBookPrice_Enter(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();
        }

        private void txtQuantity_Enter(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();
        }

        private void dgvBookList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvBookList.Rows.Clear();
        }

        private void dgvBookList_Enter(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void dgvBookList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void dgvBookList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //ClearTextFieldsBinding();

            //RebindTextFields();

            //ClearTextFieldsBinding();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            ClearTextFieldsBinding();

            RebindTextFields();

            ClearTextFieldsBinding();
        }

        private void txtBookID_Enter(object sender, EventArgs e)
        {

        }
    }
}
