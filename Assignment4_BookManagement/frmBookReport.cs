using System;
using System.Data;
using System.Windows.Forms;

namespace Assignment4_BookManagement
{
    public partial class frmBookReport : Form
    {
        private DataTable _BookTable;
        private const string SortColumnName = "BookPrice";
        private const string SortOrder = "DESC";
        public frmBookReport()
        {
            InitializeComponent();
        }
        public frmBookReport(DataTable BookTable):this()
        {
            _BookTable = BookTable;
            //DataRow row = _BookTable.NewRow();
            //lblBookReport.Text += row.RowState.ToString();
            //_BookTable.Rows.Add(row);
            //lblBookReport.Text += row.RowState.ToString();
            //_BookTable.AcceptChanges();
            //lblBookReport.Text += row.RowState.ToString();
            //_BookTable.Rows.Remove(row);
            //lblBookReport.Text += row.RowState.ToString();
        }
        private void LoadData()
        {
            DataView dataView = _BookTable.DefaultView;
            dataView.Sort = $"{SortColumnName} {SortOrder}";
            DataTable dataTable = dataView.ToTable();
            dgvBookList.DataSource = dataTable;
        }
        private void frmBookReport_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
